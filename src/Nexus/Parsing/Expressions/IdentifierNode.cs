namespace Nexus.Parsing.Expressions
{
    class IdentifierNode : Node
    {
        public string Identifier { get; }

        public IdentifierNode(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString() => Identifier;
    }
}
