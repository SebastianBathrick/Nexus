namespace Clua.Tokens;

enum TokenType
{
    None,
    NumberLiteral,
    Identifier,
    NumberKeyword,
    StringKeyword,
    BoolKeyword,
    TrueKeyword,
    FalseKeyword,
    PlusOperator,
    MinusOperator,
    MultiplyOperator,
    DivideOperator,
    EqualsOperator,
    EqualityOperator,
    InequalityOperator,
    LessThanOperator,
    GreaterThanOperator,
    LessThanOrEqualOperator,
    GreaterThanOrEqualOperator,
    LogicalAndOperator,
    LogicalOrOperator,
    LogicalNotOperator,
    OpenParen,
    CloseParen,
    CurlyOpen,
    CurlyClose
}
