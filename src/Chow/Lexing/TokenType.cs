namespace Chow.Lexing
{
    enum TokenType
    {
        None,

        NumberLiteral,
        Identifier,
        KeywordTrue,
        KeywordFalse,

        // Statements
        SymbolAssignment,

        // Arithmetic symbols
        SymbolPlus,
        SymbolMinus,
        SymbolMultiply,
        SymbolDivide,

        SymbolLess,
        SymbolGreater,
        SymbolLessEqual,
        SymbolGreaterEqual,

        // Comparison keywords / operators
        KeywordEquals,
        SymbolNotEqual,


        // Logical Keywords
        KeywordAnd,
        KeywordOr,
        KeywordNot,

        // Delimiters
        DelimiterOpenParen,
        DelimiterCloseParen,
        DelimiterCurlyOpen,
        DelimiterCurlyClose,

        // Reserved keywords
        KeywordReturn,
        KeywordIf,
    }
}
