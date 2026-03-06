namespace Nexus.SyntaxAnalysis.Expressions.Literals
{
    abstract class LiteralNode : Node
    {
        public abstract double GetNumberValue();
    }
}
