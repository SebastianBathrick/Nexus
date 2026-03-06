namespace Nexus.Parsing
{
    class SyntaxTree : Node
    {
        public SyntaxTree(Node topLevelBlockNode)
        {
            TopLevelBlockNode = topLevelBlockNode;
        }

        public Node TopLevelBlockNode { get; }

        public override string ToString() => TopLevelBlockNode.ToString();
    }
}
