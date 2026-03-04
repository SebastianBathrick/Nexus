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
        LogicalAndOperator,
        LogicalOrOperator,
        LogicalNotOperator,

        // Delimiters
        OpenParen,
        CloseParen,
        CurlyOpen,
        CurlyClose,

        // Reserved keywords
        ReturnKeyword,
    }
}
