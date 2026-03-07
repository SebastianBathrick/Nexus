using System;
using System.Collections.Generic;
using Chow.Lexing;
using Chow.Parsing.Expressions;
using Chow.Parsing.Statements;
using Chow.Values;
namespace Chow.Parsing
{
    class Parser
    {
        const int TrueBoolToNumberValue = 1;
        const int FalseBoolToNumberValue = 0;

        #region Top-Level/Block Parsing Methods

        public static Node ParseTokens(TokenCollection tkns)
        {
            var topLvlBlock = ParseBlock(tkns);

            if (!tkns.IsEmpty)
                throw new ArgumentException($"Unexpected token {tkns.Read()}");

            return new SyntaxTree(topLvlBlock);
        }

        public static Node ParseBlock(TokenCollection tkns)
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

        #endregion

        #region Statement Parsing Methods

        static bool TryParseStatement(TokenCollection tkns, out Node statementNode)
        {
            if (tkns.IsEmpty)
            {
                statementNode = null;
                return false;
            }

            // TODO: Update exceptions that enable better error reporting for client/user
            switch (tkns.PeekType())
            {
                case TokenType.KeywordReturn:
                    statementNode = ParseReturnStatement(tkns);
                    return true;
                case TokenType.Identifier:
                    statementNode = ParseAssignmentStatement(tkns);
                    return true;
                default:
                    statementNode = null;
                    return false;
            }
        }

        static Node ParseReturnStatement(TokenCollection tkns)
        {
            if (!tkns.IsOfTypeAndConsume(TokenType.KeywordReturn))
                throw new ArgumentException($"Expected {TokenType.KeywordReturn}");

            return new ReturnNode(ParseExpression(tkns));
        }

        static Node ParseAssignmentStatement(TokenCollection tkns)
        {
            if (!tkns.IsOfType(TokenType.Identifier))
                throw new ArgumentException($"Expected {TokenType.Identifier}");

            var identifier = tkns.Read().Plaintext;

            if (!tkns.IsOfTypeAndConsume(TokenType.SymbolAssignment))
                throw new ArgumentException($"Expected {TokenType.SymbolAssignment}");

            return new AssignmentNode(identifier, ParseExpression(tkns));
        }

        #endregion

        #region Parse Expression Methods

        /*
         * Operator Precedence:
         * 1. Arithmetic: *, /, +, -
         * 2. Comparison: >, >=, <, <=, is, is not
         * 3. Logic: and, or
         *
         * Example:
         * return 1 + 2 + 3 > 0 or true and false
         * return 6 > 0 or true and false
         * return true or true and false
         * return true or false
         * return true
         *
         * How it is organized in the syntax tree:
         * ((((1 + 2) + 3) > 0) or (true and false))
         *
         * Function Call Order:
         * 1. ParseExpression
         * 2. ParseOrExpression
         * 3. ParseAndExpression
         * 4. ParseComparisonExpression
         * 5. ParseArithmeticExpression
         * 6. ParseArithmeticTerm
         * 7. ParseFactor
         * 8. ParseNestedExpression
         *
         * Note: If there are parenthesis it will call ParseExpression and the call order will repeat.
         *       It will continuously reach and call nested expression until the innermost expression
         *       is reached and parsed.
         */
        static Node ParseExpression(TokenCollection tkns) => ParseOrExpression(tkns);

        static Node ParseOrExpression(TokenCollection tkns)
        {
            var left = ParseAndExpression(tkns);

            while (tkns.IsOfType(TokenType.KeywordOr))
            {
                tkns.Consume();
                var right = ParseAndExpression(tkns);
                left = new ExpressionNode(ExpressionOperator.LogicalOr, left, right);
            }

            return left;
        }

        static Node ParseAndExpression(TokenCollection tkns)
        {
            var left = ParseComparisonExpression(tkns);

            while (tkns.IsOfType(TokenType.KeywordAnd))
            {
                tkns.Consume();
                var right = ParseComparisonExpression(tkns);
                left = new ExpressionNode(ExpressionOperator.LogicalAnd, left, right);
            }

            return left;
        }

        static Node ParseComparisonExpression(TokenCollection tkns)
        {
            var left = ParseArithmeticExpression(tkns);

            while (tkns.IsOfType(
                       TokenType.SymbolGreater,
                       TokenType.SymbolGreaterEqual,
                       TokenType.SymbolLess,
                       TokenType.SymbolLessEqual,
                       TokenType.KeywordEquals,
                       TokenType.SymbolNotEqual))
            {
                var tknType = tkns.ReadType();
                ExpressionOperator opType;

                if (tknType == TokenType.KeywordEquals && tkns.PeekType() == TokenType.KeywordNot)
                {
                    opType = ExpressionOperator.Inequality;
                    tkns.Consume();
                }
                else
                {
                    opType = GetComparisonOperationType(tknType);
                }

                var right = ParseArithmeticExpression(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

        static Node ParseArithmeticExpression(TokenCollection tkns)
        {
            var left = ParseArithmeticTerm(tkns);

            while (tkns.IsOfType(TokenType.SymbolPlus, TokenType.SymbolMinus))
            {
                var opType = GetArithmeticOperationType(tkns.ReadType());
                var right = ParseArithmeticTerm(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

        static Node ParseArithmeticTerm(TokenCollection tkns)
        {
            var left = ParseFactor(tkns);

            while (tkns.IsOfType(TokenType.SymbolMultiply, TokenType.SymbolDivide))
            {
                var opType = GetArithmeticOperationType(tkns.ReadType());
                var right = ParseFactor(tkns);
                left = new ExpressionNode(opType, left, right);
            }

            return left;
        }

        static Node ParseFactor(TokenCollection tkns)
        {
            if (tkns.IsEmpty)
                throw new ArgumentException("Unexpected end of expression");

            switch (tkns.PeekType())
            {
                case TokenType.NumberLiteral:
                    return new NumberLiteralNode(double.Parse(tkns.Read().Plaintext));
                case TokenType.DelimiterOpenParen:
                    return ParseNestedExpression(tkns);
                case TokenType.SymbolMinus:
                    return ParseNegatedFactor(tkns);
                case TokenType.Identifier:
                    return new IdentifierNode(tkns.Read().Plaintext);
                case TokenType.KeywordTrue:
                    tkns.Consume();
                    return new BoolLiteralNode(true);
                case TokenType.KeywordFalse:
                    tkns.Consume();
                    return new BoolLiteralNode(false);
                case TokenType.KeywordNot:
                    tkns.Consume();
                    return new UnaryExpressionNode(ExpressionOperator.LogicalNot, ParseFactor(tkns));
                default:
                    throw new ArgumentException($"Invalid factor: {tkns.PeekType()}");
            }
        }

        static Node ParseNestedExpression(TokenCollection tkns)
        {
            // Consume the opening parenthesis
            tkns.Consume();

            var expr = ParseExpression(tkns);

            if (!tkns.IsOfTypeAndConsume(TokenType.DelimiterCloseParen))
                throw new ArgumentException("Expected ')'");

            return expr;
        }

        static Node ParseNegatedFactor(TokenCollection tkns)
        {
            // TODO: Add identifiers later
            tkns.Consume(); // Consume the '-' operator

            if (tkns.IsEmpty)
                throw new ArgumentException("Unexpected end of expression after '-' operator");

            if (tkns.IsOfType(TokenType.KeywordTrue))
            {
                tkns.Consume();
                return new NumberLiteralNode(-TrueBoolToNumberValue);
            }

            if (tkns.IsOfType(TokenType.KeywordFalse))
            {
                tkns.Consume();
                return new NumberLiteralNode(-ChowBool.FalseValue);
            }

            return new ExpressionNode(ExpressionOperator.Multiplication,
                new NumberLiteralNode(-ChowBool.TrueValue), ParseFactor(tkns));
        }

        #region TokenType to ExpressionOperator Methods

        static ExpressionOperator GetComparisonOperationType(TokenType tokenType)
        {
            /* Note: The not equals/inequality operator is handled by ParseComparisonExpression (caller)
             *       because it is split into two separate tokens ("is not"). So that's why it's absent here. */
            switch (tokenType)
            {
                case TokenType.SymbolGreater: return ExpressionOperator.GreaterThan;
                case TokenType.SymbolGreaterEqual: return ExpressionOperator.GreaterThanOrEqual;
                case TokenType.SymbolLess: return ExpressionOperator.LessThan;
                case TokenType.SymbolLessEqual: return ExpressionOperator.LessThanOrEqual;
                case TokenType.KeywordEquals: return ExpressionOperator.Equality;
                case TokenType.SymbolNotEqual: return ExpressionOperator.Inequality;
                default: throw new ArgumentException($"Invalid comparison operator: {tokenType}");
            }
        }


        static ExpressionOperator GetArithmeticOperationType(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.SymbolPlus: return ExpressionOperator.Addition;
                case TokenType.SymbolMinus: return ExpressionOperator.Subtraction;
                case TokenType.SymbolMultiply: return ExpressionOperator.Multiplication;
                case TokenType.SymbolDivide: return ExpressionOperator.Division;
                default: throw new ArgumentException($"Invalid arithmetic operator: {tokenType}");
            }
        }

        #endregion

        #endregion
    }
}
