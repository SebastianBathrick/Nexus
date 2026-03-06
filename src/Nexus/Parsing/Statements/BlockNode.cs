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
            var sb = new StringBuilder("\tBlockNode(\n");
            foreach (var node in Statements)
                sb.AppendLine($"\t\t{node}");
            
            sb.AppendLine("\t)");
            return sb.ToString();
        }
    }
}
