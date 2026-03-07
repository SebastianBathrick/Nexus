namespace Nexus.Parsing.Expressions
{
    class IdentifierNode : Node
    {
        public string Identifier { get; }

        public IdentifierNode(string identifier)
        {
            Identifier = identifier;
        }

        internal override string ToDebugString(int depth) => $"{Pad(depth)}{nameof(IdentifierNode)}({Identifier})";
    }
}
