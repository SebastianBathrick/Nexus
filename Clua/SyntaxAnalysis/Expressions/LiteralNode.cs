namespace Clua.SyntaxAnalysis.Expressions;

public abstract class LiteralNode : Node
{
    public abstract double GetNumberValue();
}
