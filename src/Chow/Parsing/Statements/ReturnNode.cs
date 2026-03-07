namespace Chow.Parsing.Statements
{
    class ReturnNode : StatementNode
    {
        public ReturnNode(Node expression)
        {
            Expression = expression;
        }

        public Node Expression { get; }

        internal override string ToDebugString(int depth)
        {
            var pad = Pad(depth);
            return $"{pad}{nameof(ReturnNode)}(\n{Expression.ToDebugString(depth + 1)}\n{pad})";
        }
    }
}
