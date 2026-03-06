using System;
namespace Nexus.Parsing.Expressions
{
    class ExpressionNode : Node
    {
        public ExpressionNode(ExpressionOperator expressionOperator, Node left, Node right)
        {
            Operator = expressionOperator;
            Left = left;
            Right = right;
        }

        public ExpressionOperator Operator { get; }
        public Node Left { get; }
        public Node Right { get; }

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

}
