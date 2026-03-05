    using System;
    using System.Collections.Generic;
    using Nexus.LexicalAnalysis.Tokens;
    using Nexus.SyntaxAnalysis.Expressions;
    using Nexus.SyntaxAnalysis.Statements;

    namespace Nexus.SyntaxAnalysis
    {
        class Parser
        {
            const int TrueBoolToNumberValue = 1;
            const int FalseBoolToNumberValue = 0;

            #region Top-Level/Block Parsing Methods
    
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

            #endregion

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
                    case TokenType.KeywordReturn:
                        statementNode = ParseReturnStatement(tkns);
                        return true;
                    default:
                        statementNode = null;
                        return false;
                }
            }

            static Node ParseReturnStatement(ITokenCollection tkns)
            {
                if (!tkns.IsOfTypeAndConsume(TokenType.KeywordReturn))
                    throw new ArgumentException($"Expected {TokenType.KeywordReturn}");

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
                 * 2. ParseLogicExpression
                 * 3. ParseComparisonExpression
                 * 5. ParseArithmeticExpression
                 * 6. ParseArithmeticTerm
                 * 7. ParseFactor
                 * 8. ParseNestedExpression
                 *
                 * Note: If there are parenthesis it will call ParseExpression and the call order will repeat.
                 *       It will continuously reach and call nested expression until the innermost expression
                 *       is reached and parsed.
                 */
            static Node ParseExpression(ITokenCollection tkns) => ParseLogicExpression(tkns);

            static Node ParseLogicExpression(ITokenCollection tkns)
            {
                var left = ParseComparisonExpression(tkns);

                while (tkns.IsOfType(TokenType.KeywordAnd, TokenType.KeywordOr))
                {
                    var opType = GetLogicalOperationType(tkns.ReadType());
                    var right = ParseComparisonExpression(tkns);
                    left = new ExpressionNode(opType, left, right);
                }

                return left;
            }

            static Node ParseComparisonExpression(ITokenCollection tkns)
            {
                var left = ParseArithmeticExpression(tkns);

                while (tkns.IsOfType(
                           TokenType.SymbolGreater, 
                           TokenType.SymbolGreaterEqual, 
                           TokenType.SymbolLess,
                           TokenType.SymbolLessEqual, 
                           TokenType.KeywordEquals))
                {
                    var tknType = tkns.ReadType();
                    ExpressionOperator opType;

                    if (tkns.PeekType() == TokenType.KeywordNot)
                    {
                        opType = ExpressionOperator.Inequality;
                        tkns.Consume();
                    }
                    else
                        opType = GetComparisonOperationType(tkns.ReadType());
                    
                    var right = ParseArithmeticExpression(tkns);
                    left = new ExpressionNode(opType, left, right);
                }

                return left;
            }

            static Node ParseArithmeticExpression(ITokenCollection tkns)
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

            static Node ParseArithmeticTerm(ITokenCollection tkns)
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

            static Node ParseFactor(ITokenCollection tkns)
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
                    case TokenType.KeywordTrue:
                        tkns.Consume();
                        return new BoolLiteralNode(true);
                    case TokenType.KeywordFalse:
                        tkns.Consume();
                        return new BoolLiteralNode(false);
                    default:
                        throw new ArgumentException($"Invalid factor: {tkns.PeekType()}");
                }
            }

            static Node ParseNestedExpression(ITokenCollection tkns)
            {
                // Consume the opening parenthesis
                tkns.Consume();

                var expr = ParseExpression(tkns);

                if (!tkns.IsOfTypeAndConsume(TokenType.DelimiterCloseParen))
                    throw new ArgumentException("Expected ')'");

                return expr;
            }

            static Node ParseNegatedFactor(ITokenCollection tkns)
            {
                // TODO: Add identifiers later
                tkns.Consume(); // Consume the '-' operator

                if (tkns.IsEmpty)
                    throw new ArgumentException("Unexpected end of expression after '-' operator");

                var tkn = tkns.Read();

                switch (tkn.Type)
                {
                    case TokenType.KeywordTrue:
                        return new NumberLiteralNode(-TrueBoolToNumberValue);
                    case TokenType.KeywordFalse:
                        return new NumberLiteralNode(-FalseBoolToNumberValue);
                    case TokenType.NumberLiteral:
                    case TokenType.DelimiterOpenParen:
                        return new ExpressionNode(ExpressionOperator.Multiplication, new NumberLiteralNode(-1), ParseFactor(tkns));
                    default:
                        throw new ArgumentException($"Invalid negation of factor: {tkn.Type}");
                }
            }

            #region TokenType to ExpressionOperator Methods

            static ExpressionOperator GetLogicalOperationType(TokenType tokenType)

            {
                return tokenType switch
                {
                    TokenType.KeywordAnd => ExpressionOperator.LogicalAnd,
                    TokenType.KeywordOr => ExpressionOperator.LogicalOr,
                    _ => throw new ArgumentException($"Invalid logical operator: {tokenType}")
                };
            }
            static ExpressionOperator GetComparisonOperationType(TokenType tokenType)
            {
                /* Note: The not equals/inequality operator is handled by ParseComparisonExpression (caller)
                 *       because it is split into two separate tokens ("is not"). So that's why it's absent here. */
                return tokenType switch
                {
                    TokenType.SymbolGreater => ExpressionOperator.GreaterThan,
                    TokenType.SymbolGreaterEqual => ExpressionOperator.GreaterThanOrEqual,
                    TokenType.SymbolLess => ExpressionOperator.LessThan,
                    TokenType.SymbolLessEqual => ExpressionOperator.LessThanOrEqual,
                    TokenType.KeywordEquals => ExpressionOperator.Equality,
                    _ => throw new ArgumentException($"Invalid comparison operator: {tokenType}")
                };
            }


            static ExpressionOperator GetArithmeticOperationType(TokenType tokenType)
            {
                return tokenType switch
                {
                    TokenType.SymbolPlus => ExpressionOperator.Addition,
                    TokenType.SymbolMinus => ExpressionOperator.Subtraction,
                    TokenType.SymbolMultiply => ExpressionOperator.Multiplication,
                    TokenType.SymbolDivide => ExpressionOperator.Division,
                    _ => throw new ArgumentException($"Invalid arithmetic operator: {tokenType}")
                };
            }

            #endregion
            #endregion

        }
    }
