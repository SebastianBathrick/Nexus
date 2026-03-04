namespace Clua.SyntaxAnalysis.Expressions;

abstract class LiteralNode : Node
{
    public abstract double GetNumberValue();
}
