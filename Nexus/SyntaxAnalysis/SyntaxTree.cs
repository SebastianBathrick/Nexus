namespace Nexus.SyntaxAnalysis
{
    class SyntaxTree : Node
    {
        public Node TopLevelBlockNode { get; }

        public SyntaxTree(Node topLevelBlockNode)
        {
            TopLevelBlockNode = topLevelBlockNode;
        }

        public override string ToString()
        {
            return TopLevelBlockNode.ToString();
        }
    }
}
