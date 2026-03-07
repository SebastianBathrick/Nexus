namespace Chow.Parsing.Expressions
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

        internal override string ToDebugString(int depth)
        {
            var pad = Pad(depth);
            var content = Pad(depth + 1);
            return $"{pad}{nameof(UnaryExpressionNode)}(\n{content}{ExpressionNode.GetOperatorLabel(Operator)}\n{Operand.ToDebugString(depth + 1)}\n{pad})";
        }
    }
}
