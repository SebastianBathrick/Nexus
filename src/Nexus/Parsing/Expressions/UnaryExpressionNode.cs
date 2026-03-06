namespace Nexus.Parsing.Expressions
{
    class UnaryExpressionNode : Node
    {
        public UnaryExpressionNode(ExpressionOperator expressionOperator, Node operand)
        {
            Operator = expressionOperator;
            Operand = operand;
        }

        public ExpressionOperator Operator { get; }
        public Node Operand { get; }

        public override string ToString() => Operator switch
        {
            ExpressionOperator.LogicalNot => $"UnaryExpressionNode(!{Operand})",
            _ => $"UnaryExpressionNode({Operator} {Operand})"
        };
    }
}
