using System;
using System.Collections.Generic;
using Nexus.SyntaxAnalysis.Expressions;
using Nexus.Tokens;

namespace Nexus.SyntaxAnalysis
{
    class Parser
    {
        public static Node ParseTokens(ITokenCollection tkns)
        {
            var topLvlBlock = ParseBlock(tkns);

            if (!tkns.IsEmpty)
                throw new ArgumentException($"Unexpected token {tkns.Read()}");

            return new SyntaxTree(topLvlBlock);
        }

        public static Node ParseBlock(ITokenCollection tkns)
        {
            var statements = new List<Node>();

            while (TryParseStatement(tkns, out var statementNode))
            {
                if (statementNode == null)
                    throw new NullReferenceException($"{nameof(TryParseStatement)} returned true but statementNode is null");

                statements.Add(statementNode);
            }

            return new BlockNode(statements.ToArray());
        }

        #region Statement Parsing Methods

        static bool TryParseStatement(ITokenCollection tkns, out Node? statementNode)
        {
            if (tkns.IsEmpty)
            {
                statementNode = null;
                return false;
            }

            // TODO: Update exceptions that enable better error reporting for client/user
            switch (tkns.PeekType())
            {
                case TokenType.ReturnKeyword:
                    statementNode = ParseReturnStatement(tkns);
                    return true;
                default:
                    statementNode = null;
                    return false;
            }
        }

        static Node ParseReturnStatement(ITokenCollection tkns)
        {
            if (!tkns.IsOfTypeAndConsume(TokenType.ReturnKeyword))
                throw new ArgumentException($"Expected {TokenType.ReturnKeyword}");

            return new ReturnNode(ParseExpression(tkns));
        }

        #endregion

        #region Parse Expression Methods

        /*
         * Operator Precedence:
         * 1. Arithmetic: *, /, +, -
         * 2. Comparison: >, >=, <, <=, is, is not
         * 3. Logic: and, or
         *
         * Function Call Order:
         * 1. ParseExpression
         * 2. ParseLogicExpression
         * 3. ParseLogicTerm (terms are made up of comparison expressions)
         * 4. ParseComparisonExpression
         * 5. ParseArithmeticExpression
         * 6. ParseArithmeticTerm
         * 7. ParseFactor
         * 8. ParseNestedExpression
         *
         * Note: If there are parenthesis it will call ParseExpression and the call order will repeat.
         *       It will continuously reach and call nested expression until the inner most expression
         *       is reached and parsed.
        */

        static Node ParseExpression(ITokenCollection tkns) => ParseLogicExpression(tkns);

        static Node ParseLogicExpression(ITokenCollection tkns)
        {
            var left = ParseComparisonExpression(tkns);

            while (tkns.IsOfType(TokenType.LogicalOr, TokenType.LogicalNot))
            {
                var opType = GetLogicOperationType(tkns.ReadType());
                var right = ParseComparisonExpression(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

            static Node ParseComparisonExpression(ITokenCollection tkns)
        {
            var left = ParseArithmeticExpression(tkns);

            while (tkns.IsOfType(TokenType.GreaterThanOperator, TokenType.GreaterThanOrEqualOperator, TokenType.LessThanOperator, TokenType.LessThanOrEqualOperator, TokenType.EqualityOperator, TokenType.InequalityOperator))
            {
                var opType = GetComparisonOperationType(tkns.ReadType());
                var right = ParseArithmeticExpression(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

        static Node ParseArithmeticExpression(ITokenCollection tkns)
        {
            var left = ParseArithmeticTerm(tkns);

            while (tkns.IsOfType(TokenType.PlusOperator, TokenType.MinusOperator))
            {
                var opType = GetArithmeticOperationType(tkns.ReadType());
                var right = ParseArithmeticTerm(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }





        static Node ParseNestedExpression(ITokenCollection tkns)
        {
            // Consume the opening parenthesis
            tkns.Consume();

            var expr = ParseExpression(tkns);

            if (!tkns.IsOfTypeAndConsume(TokenType.CloseParen))
                throw new ArgumentException("Expected ')'");

            return expr;
        }


        static Node ParseLogicTerm(ITokenCollection tkns)
        {
            var left = ParseComparisonExpression(tkns);

            while (tkns.IsOfType(TokenType.LogicalAnd))
            {
                var opType = GetComparisonOperationType(tkns.ReadType());
                var right = ParseComparisonExpression(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

        static Node ParseArithmeticTerm(ITokenCollection tkns)
        {
            var left = ParseFactor(tkns);

            while (tkns.IsOfType(TokenType.MultiplyOperator, TokenType.DivideOperator))
            {
                var opType = GetArithmeticOperationType(tkns.ReadType());
                var right = ParseFactor(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

        #endregion

        #region Parse Factor Methods

        static Node ParseFactor(ITokenCollection tkns)
        {
            if (tkns.IsEmpty)
                throw new ArgumentException("Unexpected end of expression");

            switch (tkns.PeekType())
            {
                case TokenType.NumberLiteral:
                    return new NumberLiteralNode(double.Parse(tkns.Read().Plaintext));
                case TokenType.TrueKeyword:
                    tkns.Consume();
                    return new BoolLiteralNode(true);
                case TokenType.FalseKeyword:
                    tkns.Consume();
                    return new BoolLiteralNode(false);
                case TokenType.OpenParen:
                    return ParseNestedExpression(tkns);
                case TokenType.MinusOperator:
                    return ParseNegatedFactor(tkns);
                case TokenType.LogicalNot:
                    return ParseNotFactor(tkns);
                default:
                    throw new ArgumentException($"Invalid factor: {tkns.ReadType()}");
            }
        }

        static Node ParseNegatedFactor(ITokenCollection tkns)
        {
            tkns.Consume(); // Consume the '-' operator

            if (tkns.IsEmpty)
                throw new ArgumentException("Unexpected end of expression after '-' operator");

            // Add identifiers
            if (!tkns.IsOfType(TokenType.NumberLiteral, TokenType.OpenParen))
                throw new ArgumentException($"Invalid negation of factor: {tkns.ReadType()}");

            return new ExpressionNode(ExpressionOperator.Multiplication, new NumberLiteralNode(-1), ParseFactor(tkns));
        }

        static Node ParseNotFactor(ITokenCollection tkns)
        {
            if (!tkns.IsOfTypeAndConsume(TokenType.LogicalNot))
                throw new ArgumentException($"Expected {TokenType.LogicalNot}");

            if (tkns.IsEmpty)
                throw new ArgumentException("Unexpected end of expression after 'not' operator");

            return new UnaryExpressionNode(ExpressionOperator.LogicalNot, ParseFactor(tkns));
        }

        #endregion

        #region Get Operation Type Methods
        
        static ExpressionOperator GetArithmeticOperationType(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.PlusOperator => ExpressionOperator.Addition,
                TokenType.MinusOperator => ExpressionOperator.Subtraction,
                TokenType.MultiplyOperator => ExpressionOperator.Multiplication,
                TokenType.DivideOperator => ExpressionOperator.Division,
                _ => throw new ArgumentException($"Invalid arithmetic operator: {tokenType}")
            };
        }

        static ExpressionOperator GetComparisonOperationType(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.GreaterThanOperator => ExpressionOperator.GreaterThan,
                TokenType.GreaterThanOrEqualOperator => ExpressionOperator.GreaterThanOrEqual,
                TokenType.LessThanOperator => ExpressionOperator.LessThan,
                TokenType.LessThanOrEqualOperator => ExpressionOperator.LessThanOrEqual,
                TokenType.EqualityOperator => ExpressionOperator.Equality,
                TokenType.InequalityOperator => ExpressionOperator.Inequality,
                _ => throw new ArgumentException($"Invalid comparison operator: {tokenType}")
            };
        }

        static ExpressionOperator GetLogicOperationType(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.LogicalAnd => ExpressionOperator.LogicalAnd,
                TokenType.LogicalOr => ExpressionOperator.LogicalOr,
                TokenType.LogicalNot => ExpressionOperator.LogicalNot,
                _ => throw new ArgumentException($"Invalid logic operator: {tokenType}")
            };
        }

        #endregion
    }
}
