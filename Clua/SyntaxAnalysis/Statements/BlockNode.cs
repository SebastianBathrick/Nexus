using System.Text;

namespace Clua.SyntaxAnalysis
{
    class BlockNode : Node
    {
        public Node[] Statements { get; }

        public BlockNode(Node[] statements)
        {
            Statements = statements;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var node in Statements)
                sb.AppendLine(node.ToString());
            return sb.ToString();
        }
    }
}
