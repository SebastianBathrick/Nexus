namespace Clua.AbstractSyntaxTree;

public class ExpressionNode(Operator @operator, Node left, Node right) : Node
{
    public Operator Operator { get; init; } = @operator;
    public Node Left { get; } = left;
    public Node Right { get; } = right;

    public override string ToString() => $"({Left} {GetOperatorString()} {Right})";

    string GetOperatorString()
    {
        return Operator switch
        {
            Operator.Addition => "+",
            Operator.Subtraction => "-",
            Operator.Multiplication => "*",
            Operator.Division => "/",
            Operator.LogicalAnd => "&&",
            Operator.LogicalOr => "||",
            Operator.GreaterThan => ">",
            Operator.GreaterThanOrEqual => ">=",
            Operator.LessThan => "<",
            Operator.LessThanOrEqual => "<=",
            Operator.Equality => "==",
            Operator.Inequality => "!=",
            _ => throw new InvalidOperationException($"Unsupported operation type: {Operator}")
        };
    }
}

public enum Operator
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