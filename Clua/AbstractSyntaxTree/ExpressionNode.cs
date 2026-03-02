namespace Clua.AbstractSyntaxTree;

public class ExpressionNode(Operator op, Node left, Node right) : Node
{
    Operator Op { get; init; } = op;
    Node Left { get; init; } = left;
    Node Right { get; init; } = right;

    public override string ToString() => $"({Left} {GetOperatorString()} {Right})";

    string GetOperatorString()
    {
        return Op switch
        {
            Operator.Add => "+",
            Operator.Subtract => "-",
            Operator.Multiply => "*",
            Operator.Divide => "/",
            Operator.LogicalAnd => "&&",
            Operator.LogicalOr => "||",
            Operator.GreaterThan => ">",
            Operator.GreaterThanOrEqual => ">=",
            Operator.LessThan => "<",
            Operator.LessThanOrEqual => "<=",
            Operator.Equality => "==",
            Operator.Inequality => "!=",
            _ => throw new InvalidOperationException($"Unsupported operation type: {Op}")
        };
    }
}

public enum Operator
{
    Add,
    Subtract,
    Multiply,
    Divide,
    LogicalAnd,
    LogicalOr,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Equality,
    Inequality,
}