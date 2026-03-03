namespace Clua.SyntaxAnalysis;

class SyntaxTree(Node topLevelBlockNode) : Node
{
    public Node TopLevelBlockNode { get; } = topLevelBlockNode;
    
    public override string ToString()
    {
        return TopLevelBlockNode.ToString();
    }
}
