namespace Nexus.SyntaxAnalysis.Statements
{
    class IfNode : StatementNode
    {
        public Node Condition { get; }
        public Node ThenBranch { get; }
        public Node? ElseBranch { get; }

        public override string ToString() => $"if {Condition} then {ThenBranch} else {ElseBranch}";
    }
}