using Clua.SyntaxTree;
using Clua.Tokens;

namespace Clua;

class Parser
{
    public static Node Parse(ITokenCollection tkns)
    {
        return ParseExpression(tkns);
    }
    
    static Node ParseExpression(ITokenCollection tkns) => ParseLogicExpression(tkns);
    
    static Node ParseLogicExpression(ITokenCollection tkns)
    {
        var left = ParseLogicTerm(tkns);

        while (tkns.IsOfType(TokenType.LogicalAndOperator, TokenType.LogicalOrOperator))
        {
            var opType = tkns.ReadType() == TokenType.LogicalAndOperator ? Operator.LogicalAnd : Operator.LogicalOr;
            var right = ParseLogicTerm(tkns);
            left = new ExpressionNode(opType, left, right);
        }
        
        return left;
    }
    
    static Node ParseLogicTerm(ITokenCollection tkns)
    {
        var left = ParseArithmeticExpression(tkns);

        while (tkns.IsOfType(TokenType.GreaterThanOperator, TokenType.GreaterThanOrEqualOperator, TokenType.LessThanOperator, 
                   TokenType.LessThanOrEqualOperator, TokenType.EqualityOperator, TokenType.InequalityOperator))
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
    
    static Node ParseFactor(ITokenCollection tkns)
    {
        if (tkns.IsEmpty)
            throw new ArgumentException("Unexpected end of expression");
        
        switch (tkns.PeekType())
        {
            case TokenType.NumberLiteral:
                return new NumberLiteralNode(double.Parse(tkns.Read().Plaintext));
            case TokenType.OpenParen:
                return ParseNestedExpression(tkns);
            case TokenType.MinusOperator:
                return ParseNegatedFactor(tkns);
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

        return new ExpressionNode(Operator.Multiplication, new NumberLiteralNode(-1), ParseFactor(tkns));
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
    
    static Operator GetArithmeticOperationType(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.PlusOperator => Operator.Addition,
            TokenType.MinusOperator => Operator.Subtraction,
            TokenType.MultiplyOperator => Operator.Multiplication,
            TokenType.DivideOperator => Operator.Division,
            _ => throw new ArgumentException($"Invalid arithmetic operator: {tokenType}")
        };
    }
    
    static Operator GetComparisonOperationType(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.GreaterThanOperator => Operator.GreaterThan,
            TokenType.GreaterThanOrEqualOperator => Operator.GreaterThanOrEqual,
            TokenType.LessThanOperator => Operator.LessThan,
            TokenType.LessThanOrEqualOperator => Operator.LessThanOrEqual,
            TokenType.EqualityOperator => Operator.Equality,
            TokenType.InequalityOperator => Operator.Inequality,
            _ => throw new ArgumentException($"Invalid comparison operator: {tokenType}")
        };
    }
}
