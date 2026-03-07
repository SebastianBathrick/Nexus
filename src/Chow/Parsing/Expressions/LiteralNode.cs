namespace Chow.Parsing.Expressions
{
    abstract class LiteralNode : Node
    {
        public abstract double GetNumberValue();

        internal abstract override string ToDebugString(int depth);
    }
}
