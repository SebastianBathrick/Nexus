namespace Chow.Parsing.Statements
{
    class AssignmentNode : StatementNode
    {
        public AssignmentNode(string identifier, Node expression)
        {
            Identifier = identifier;
            Expression = expression;
            IsImplicitDeclaration = false;
        }

        public string Identifier { get; }
        public Node Expression { get; }
        public bool IsImplicitDeclaration { get; }

        internal override string ToDebugString(int depth)
        {
            var pad = Pad(depth);
            return $"{pad}{nameof(AssignmentNode)}({Identifier},\n{Expression.ToDebugString(depth + 1)}\n{pad})";
        }
    }
}
