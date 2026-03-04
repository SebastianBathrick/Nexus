using System.Text;

namespace Clua.SyntaxAnalysis
{
    class BlockNode(Node[] statements) : Node
    {
        public Node[] Statements => statements;

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var node in statements)
                sb.AppendLine(node.ToString());
            return sb.ToString();
        }
    }
}
