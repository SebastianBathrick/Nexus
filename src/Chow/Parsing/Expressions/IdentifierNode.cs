namespace Chow.Parsing.Expressions
{
    class IdentifierNode : Node
    {
        public IdentifierNode(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        internal override string ToDebugString(int depth) => $"{Pad(depth)}{nameof(IdentifierNode)}({Identifier})";
    }
}
