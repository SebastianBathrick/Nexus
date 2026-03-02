namespace Clua;

internal class SyntaxSpecSheet
{
    public static readonly IReadOnlyDictionary<string, TokenType> ReservedKeywords = new Dictionary<string, TokenType>()
    {
        {"float", TokenType.FloatKeyword },
        {"int", TokenType.IntKeyword },
        {"string", TokenType.StringKeyword },
        {"bool", TokenType.BoolKeyword },
        {"true", TokenType.TrueKeyword },
        {"false", TokenType.FalseKeyword }
    };
    
    public static readonly IReadOnlyDictionary<string, TokenType> ValidOperators = new Dictionary<string, TokenType>()
    {
        {"+", TokenType.PlusOperator },
        {"-", TokenType.MinusOperator },
        {"*", TokenType.MultiplyOperator },
        {"/", TokenType.DivideOperator },
        {"=", TokenType.EqualsOperator },
        {"==", TokenType.EqualityOperator },
        {"!=", TokenType.InequalityOperator },
        {"<", TokenType.LessThanOperator },
        {">", TokenType.GreaterThanOperator },
        {"<=", TokenType.LessThanOrEqualOperator },
        {">=", TokenType.GreaterThanOrEqualOperator },
        {"&&", TokenType.LogicalAndOperator },
        {"||", TokenType.LogicalOrOperator },
        {"!", TokenType.LogicalNotOperator }
    };

    public static CharType GetCharType(char c)
    {
        return c switch
        {
            // 0-9
            '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' => CharType.Numeric,
            
            // aA-zZ
            'a' or 'b' or 'c' or 'd' or 'e' or 'f' or 'g' or 'h' or 'i' or 'j' or 'k' or 'l' or 'm' or 'n' or 'o' or 'p' or 'q' or 'r' or 's' or 't' or 'u' or 'v' or 'w'
                or 'x' or 'y' or 'z' or 'A' or 'B' or 'C' or 'D' or 'E' or 'F' or 'G' or 'H' or 'I' or 'J' or 'K' or 'L' or 'M' or 'N' or 'O' or 'P' or 'Q' or 'R' or 'S'
                or 'T' or 'U' or 'V' or 'W' or 'X' or 'Y' or 'Z' => CharType.Alpha,
            
            // Whitespace characters (Same valid whitespace as C compiler)
            ' ' or '\t' or '\n' or '\r' or '\v' or '\f' => CharType.Whitespace,
            
            // Valid Operators
            '+' or '-' or '*' or '/' or '=' or '!' or '<' or '>' or '&' or '|' => CharType.Operator,
            '_' => CharType.Underscore,
            '.' => CharType.Dot,
            _ => CharType.Invalid
        };
    }
}