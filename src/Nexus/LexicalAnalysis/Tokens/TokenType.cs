namespace Nexus.LexicalAnalysis.Tokens
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

        // Comparison keywords
        KeywordEquals,


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
    }
}
