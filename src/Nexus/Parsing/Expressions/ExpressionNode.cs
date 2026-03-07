using System;

namespace Nexus.Parsing.Expressions
{
    class ExpressionNode : Node
    {
        const string OpAdd              = "ADD";
        const string OpSubtract         = "SUBTRACT";
        const string OpMultiply         = "MULTIPLY";
        const string OpDivide           = "DIVIDE";
        const string OpAnd              = "AND";
        const string OpOr               = "OR";
        const string OpGreaterThan      = "GREATER_THAN";
        const string OpGreaterThanEqual = "GREATER_THAN_OR_EQUAL";
        const string OpLessThan         = "LESS_THAN";
        const string OpLessThanEqual    = "LESS_THAN_OR_EQUAL";
        const string OpEquality         = "EQUAL";
        const string OpInequality       = "NOT_EQUAL";
        const string OpNot              = "NOT";

        public ExpressionNode(ExpressionOperator expressionOperator, Node left, Node right)
        {
            Operator = expressionOperator;
            Left = left;
            Right = right;
        }

        public ExpressionOperator Operator { get; }
        public Node Left { get; }
        public Node Right { get; }

        internal static string GetOperatorLabel(ExpressionOperator op) => op switch
        {
            ExpressionOperator.Addition           => OpAdd,
            ExpressionOperator.Subtraction        => OpSubtract,
            ExpressionOperator.Multiplication     => OpMultiply,
            ExpressionOperator.Division           => OpDivide,
            ExpressionOperator.LogicalAnd         => OpAnd,
            ExpressionOperator.LogicalOr          => OpOr,
            ExpressionOperator.GreaterThan        => OpGreaterThan,
            ExpressionOperator.GreaterThanOrEqual => OpGreaterThanEqual,
            ExpressionOperator.LessThan           => OpLessThan,
            ExpressionOperator.LessThanOrEqual    => OpLessThanEqual,
            ExpressionOperator.Equality           => OpEquality,
            ExpressionOperator.Inequality         => OpInequality,
            ExpressionOperator.LogicalNot         => OpNot,
            _ => throw new InvalidOperationException($"Unsupported operator: {op}")
        };

        internal override string ToDebugString(int depth)
        {
            var pad     = Pad(depth);
            var content = Pad(depth + 1);
            return $"{pad}{nameof(ExpressionNode)}(\n{content}{GetOperatorLabel(Operator)}\n{Left.ToDebugString(depth + 1)}\n{Right.ToDebugString(depth + 1)}\n{pad})";
        }
    }
}
