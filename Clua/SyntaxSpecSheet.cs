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
}