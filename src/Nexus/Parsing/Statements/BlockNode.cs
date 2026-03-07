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

        internal override string ToDebugString(int depth)
        {
            var pad = Pad(depth);
            var sb = new StringBuilder($"{pad}{nameof(BlockNode)}(\n");
            foreach (var node in Statements)
                sb.AppendLine(node.ToDebugString(depth + 1));
            sb.Append($"{pad})");
            return sb.ToString();
        }
    }
}
