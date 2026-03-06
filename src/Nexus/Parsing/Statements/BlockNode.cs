using System.Text;

namespace Nexus.Parsing.Statements
{
    class BlockNode : Node
    {
        public BlockNode(Node[] statements)
        {
            Statements = statements;
        }

        public Node[] Statements { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var node in Statements)
                sb.AppendLine(node.ToString());

            return sb.ToString();
        }
    }
}
