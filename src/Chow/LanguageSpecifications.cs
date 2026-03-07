using System.Collections.Generic;
using Chow.Lexing;
namespace Chow
{
    static class LanguageSpecifications
    {
        public static readonly IReadOnlyDictionary<string, TokenType> ReservedKeywords = new Dictionary<string, TokenType>
        {
            { "return", TokenType.KeywordReturn },
            { "true", TokenType.KeywordTrue },
            { "false", TokenType.KeywordFalse },
            { "and", TokenType.KeywordAnd },
            { "or", TokenType.KeywordOr },
            { "is", TokenType.KeywordEquals },
            { "not", TokenType.KeywordNot }
        };

        public static readonly IReadOnlyDictionary<string, TokenType> Operators = new Dictionary<string, TokenType>
        {
            { "+", TokenType.SymbolPlus },
            { "-", TokenType.SymbolMinus },
            { "*", TokenType.SymbolMultiply },
            { "/", TokenType.SymbolDivide },
            { "=", TokenType.SymbolAssignment },
            { "<", TokenType.SymbolLess },
            { ">", TokenType.SymbolGreater },
            { "<=", TokenType.SymbolLessEqual },
            { ">=", TokenType.SymbolGreaterEqual },
            { "==", TokenType.KeywordEquals },
            { "!=", TokenType.SymbolNotEqual }
        };

        public static readonly IReadOnlyDictionary<CharType, TokenType> Delimeters = new Dictionary<CharType, TokenType>
        {
            { CharType.OpenParen, TokenType.DelimiterOpenParen },
            { CharType.CloseParen, TokenType.DelimiterCloseParen },
            { CharType.CurlyOpen, TokenType.DelimiterCurlyOpen },
            { CharType.CurlyClose, TokenType.DelimiterCurlyClose }
        };

        public static CharType GetCharType(char c)
        {
            switch (c)
            {
                // 0-9
                case '0': case '1': case '2': case '3': case '4':
                case '5': case '6': case '7': case '8': case '9':
                    return CharType.Numeric;

                // aA-zZ
                case 'a': case 'b': case 'c': case 'd': case 'e': case 'f': case 'g': case 'h': case 'i': case 'j':
                case 'k': case 'l': case 'm': case 'n': case 'o': case 'p': case 'q': case 'r': case 's': case 't':
                case 'u': case 'v': case 'w': case 'x': case 'y': case 'z':
                case 'A': case 'B': case 'C': case 'D': case 'E': case 'F': case 'G': case 'H': case 'I': case 'J':
                case 'K': case 'L': case 'M': case 'N': case 'O': case 'P': case 'Q': case 'R': case 'S': case 'T':
                case 'U': case 'V': case 'W': case 'X': case 'Y': case 'Z':
                    return CharType.Alpha;

                // Whitespace characters (Same valid whitespace as C compiler)
                case ' ': case '\t': case '\n': case '\r': case '\v': case '\f':
                    return CharType.Whitespace;

                // Valid Operators
                case '+': case '-': case '*': case '/': case '=': case '!': case '<': case '>': case '&': case '|':
                    return CharType.Operator;

                case '_': return CharType.Underscore;
                case '.': return CharType.Dot;
                case '(': return CharType.OpenParen;
                case ')': return CharType.CloseParen;
                case '{': return CharType.CurlyOpen;
                case '}': return CharType.CurlyClose;
                default: return CharType.Invalid;
            }
        }
    }

    public enum CharType
    {
        Numeric,
        Alpha,
        Underscore,
        Whitespace,
        Dot,
        Operator,
        OpenParen,
        CloseParen,
        CurlyOpen,
        CurlyClose,
        Invalid
    }
}
