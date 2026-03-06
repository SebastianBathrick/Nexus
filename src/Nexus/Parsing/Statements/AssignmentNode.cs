namespace Nexus.Parsing.Statements
{
    class AssignmentNode : StatementNode
    {
        public string Identifier { get; }
        public Node Expression { get; }
        public bool IsImplicitDeclaration { get; }

        public AssignmentNode(string identifier, Node expression)
        {
            Identifier = identifier;
            Expression = expression;
            IsImplicitDeclaration = false;
        }

        public override string ToString() => $"{Identifier} = {Expression}";
    }
}