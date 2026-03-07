using System;
namespace Chow.Parsing.Expressions
{
    class ExpressionNode : Node
    {
        const string OpAdd = "ADD";
        const string OpSubtract = "SUBTRACT";
        const string OpMultiply = "MULTIPLY";
        const string OpDivide = "DIVIDE";
        const string OpAnd = "AND";
        const string OpOr = "OR";
        const string OpGreaterThan = "GREATER_THAN";
        const string OpGreaterThanEqual = "GREATER_THAN_OR_EQUAL";
        const string OpLessThan = "LESS_THAN";
        const string OpLessThanEqual = "LESS_THAN_OR_EQUAL";
        const string OpEquality = "EQUAL";
        const string OpInequality = "NOT_EQUAL";
        const string OpNot = "NOT";

        public ExpressionNode(ExpressionOperator expressionOperator, Node left, Node right)
        {
            Operator = expressionOperator;
            Left = left;
            Right = right;
        }

        public ExpressionOperator Operator { get; }
        public Node Left { get; }
        public Node Right { get; }

        internal static string GetOperatorLabel(ExpressionOperator op)
        {
            switch (op)
            {
                case ExpressionOperator.Addition: return OpAdd;
                case ExpressionOperator.Subtraction: return OpSubtract;
                case ExpressionOperator.Multiplication: return OpMultiply;
                case ExpressionOperator.Division: return OpDivide;
                case ExpressionOperator.LogicalAnd: return OpAnd;
                case ExpressionOperator.LogicalOr: return OpOr;
                case ExpressionOperator.GreaterThan: return OpGreaterThan;
                case ExpressionOperator.GreaterThanOrEqual: return OpGreaterThanEqual;
                case ExpressionOperator.LessThan: return OpLessThan;
                case ExpressionOperator.LessThanOrEqual: return OpLessThanEqual;
                case ExpressionOperator.Equality: return OpEquality;
                case ExpressionOperator.Inequality: return OpInequality;
                case ExpressionOperator.LogicalNot: return OpNot;
                default: throw new InvalidOperationException($"Unsupported operator: {op}");
            }
        }

        internal override string ToDebugString(int depth)
        {
            var pad = Pad(depth);
            var content = Pad(depth + 1);
            return $"{pad}{nameof(ExpressionNode)}(\n{content}{GetOperatorLabel(Operator)}\n{Left.ToDebugString(depth + 1)}\n{Right.ToDebugString(depth + 1)}\n{pad})";
        }
    }
}
