namespace Nexus.Tokens
{
    enum TokenType
    {
        None,

        NumberLiteral,
        Identifier,
        TrueKeyword,
        FalseKeyword,

        // Arithmetic operators
        PlusOperator,
        MinusOperator,
        MultiplyOperator,
        DivideOperator,

        // Comparison operators
        EqualsOperator,
        EqualityOperator,
        InequalityOperator,
        LessThanOperator,
        GreaterThanOperator,
        LessThanOrEqualOperator,
        GreaterThanOrEqualOperator,

        // Logical operators
        LogicalAnd,
        LogicalOr,
        LogicalNot,

        // Delimiters
        OpenParen,
        CloseParen,
        CurlyOpen,
        CurlyClose,

        // Reserved keywords
        ReturnKeyword,
    }
}
