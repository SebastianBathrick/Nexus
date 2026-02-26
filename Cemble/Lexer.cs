namespace Cemble;

class Lexer
{
    static readonly IReadOnlyDictionary<string, TokenType> ReservedKeywords = new Dictionary<string, TokenType>()
    {
        {"float", TokenType.FloatKeyword },
        {"int", TokenType.IntKeyword },
        {"string", TokenType.StringKeyword },
        {"bool", TokenType.BoolKeyword },
        {"true", TokenType.TrueKeyword },
        {"false", TokenType.FalseKeyword }
    };
    
    public static IReadOnlyList<Token> Lex(string srcCode)
    {
        var tokens = new List<Token>();
        var stream = new CharStream(srcCode);

        while (stream.IsCharInStream())
        {
            if (stream.IsCharNumeric())
            {
                tokens.Add(ReadNumber(stream));
                continue;
            }

            if (stream.IsCharAlpha() || stream.IsCharUnderscore())
            {
                tokens.Add(ReadIdentifier(stream));
                continue;
            }

            stream.ReadNextChar();
        }

        return tokens;
    }

    static Token ReadIdentifier(CharStream stream)
    {
        var builder = new TokenBuilder(TokenType.Identifier);

        while (stream.IsCharAlpha() || stream.IsCharUnderscore() || stream.IsCharNumeric())
            builder.Append(stream.ReadNextChar());

        if (ReservedKeywords.TryGetValue(builder.ToString(), out TokenType keywordType))
            return builder.Build(keywordType);

        return builder.Build();
    }

    static Token ReadNumber(CharStream stream)
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);

        while (stream.IsCharNumeric())
            builder.Append(stream.ReadNextChar());

        if (!stream.IsCharDot())
            return builder.Build();

        // Consume the '.'
        stream.ReadNextChar();

        if (!stream.IsCharNumeric())
            throw new ArgumentException($"Invalid float literal '{builder}.' — expected digit after decimal point");

        builder.Append('.');
        builder.SetType(TokenType.FloatLiteral);

        while (stream.IsCharNumeric())
            builder.Append(stream.ReadNextChar());

        if (stream.IsCharDot())
            throw new ArgumentException($"Invalid float literal '{builder}' — unexpected '.'");

        return builder.Build();
    }
}

internal struct Token
{
    // Return empty string each time, because the class should only retrieve/store the plaintext of specific token types
    public string Plaintext => _plaintext ?? string.Empty;
    public TokenType Type { get; init; }

    private readonly string? _plaintext;

    public Token(TokenType type, string? plaintext = null)
    {
        _plaintext = plaintext;
        Type = type;
    }
}

internal enum TokenType
{
    None,
    IntLiteral,
    FloatLiteral,
    Identifier,
    IntKeyword,
    FloatKeyword,
    StringKeyword,
    BoolKeyword,
    TrueKeyword,
    FalseKeyword
}