namespace Nexus.SyntaxAnalysis.Statements
{
    class AssignmentNode : StatementNode
    {
        public string Identifier { get; }
        public Node Expression { get; }

        public AssignmentNode(string identifier, Node expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override string ToString() => $"{Identifier} = {Expression}";
    }
}