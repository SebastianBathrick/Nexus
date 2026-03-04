using System;

namespace Nexus.SyntaxAnalysis.Expressions
{
    class UnaryExpressionNode : Node
    {
        public ExpressionOperator Operator { get; }
        public Node Operand { get; }

        public UnaryExpressionNode(ExpressionOperator @operator, Node operand)
        {
            Operator = @operator;
            Operand = operand;
        }

        public override string ToString() => Operator switch
        {
            ExpressionOperator.LogicalNot => $"(!{Operand})",
            _ => $"({Operator} {Operand})"
        };
    }
}
