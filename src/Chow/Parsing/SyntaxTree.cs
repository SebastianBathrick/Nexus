namespace Chow.Parsing
{
    class SyntaxTree : Node
    {
        public SyntaxTree(Node topLevelBlockNode)
        {
            TopLevelBlockNode = topLevelBlockNode;
        }

        public Node TopLevelBlockNode { get; }

        internal override string ToDebugString(int depth) => TopLevelBlockNode.ToDebugString(depth);
    }
}
