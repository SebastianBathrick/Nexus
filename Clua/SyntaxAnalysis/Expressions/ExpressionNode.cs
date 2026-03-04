namespace Clua.SyntaxAnalysis. Expressions
{
    class ExpressionNode(ExpressionOperator @operator, Node left, Node right) : Node
    {
        public ExpressionOperator Operator { get; } = @operator;
        public Node Left { get; } = left;
        public Node Right { get; } = right;

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
    }
}
