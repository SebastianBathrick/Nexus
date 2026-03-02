namespace Clua.AbstractSyntaxTree;

public class OperationNode(OperationType type, Node left, Node right) : Node
{
    OperationType Type { get; init; } = type;
    Node Left { get; init; } = left;
    Node Right { get; init; } = right;

    public override string ToString() => $"({Left} {GetOperatorString()} {Right})";

    string GetOperatorString()
    {
        return Type switch
        {
            OperationType.Add => "+",
            OperationType.Subtract => "-",
            OperationType.Multiply => "*",
            OperationType.Divide => "/",
            OperationType.LogicalAnd => "&&",
            OperationType.LogicalOr => "||",
            OperationType.GreaterThan => ">",
            OperationType.GreaterThanOrEqual => ">=",
            OperationType.LessThan => "<",
            OperationType.LessThanOrEqual => "<=",
            OperationType.Equality => "==",
            OperationType.Inequality => "!=",
            _ => throw new InvalidOperationException($"Unsupported operation type: {Type}")
        };
    }
}

public enum OperationType
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