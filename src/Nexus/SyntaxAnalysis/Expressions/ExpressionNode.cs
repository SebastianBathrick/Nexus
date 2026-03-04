using System;

namespace Nexus.SyntaxAnalysis.Expressions
{
    class ExpressionNode : Node
    {
        public ExpressionOperator Operator { get; }
        public Node Left { get; }
        public Node Right { get; }

        public ExpressionNode(ExpressionOperator @operator, Node left, Node right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        string GetOperatorString()
        {
            return Operator switch
            {
                ExpressionOperator.Addition => "+",
                ExpressionOperator.Subtraction => "-",
                ExpressionOperator.Multiplication => "*",
                ExpressionOperator.Division => "/",
                ExpressionOperator.LogicalAnd => "&&",
                ExpressionOperator.LogicalOr => "||",
                ExpressionOperator.GreaterThan => ">",
                ExpressionOperator.GreaterThanOrEqual => ">=",
                ExpressionOperator.LessThan => "<",
                ExpressionOperator.LessThanOrEqual => "<=",
                ExpressionOperator.Equality => "==",
                ExpressionOperator.Inequality => "!=",
                ExpressionOperator.LogicalNot => "!",
                _ => throw new InvalidOperationException($"Unsupported operation type: {Operator}")
            };
        }

        public override string ToString() => $"({Left} {GetOperatorString()} {Right})";
    }

    enum ExpressionOperator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        LogicalAnd,
        LogicalOr,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Equality,
        Inequality,
        LogicalNot,
    }
}
