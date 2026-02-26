namespace Cemble;

class Lexer
{
    public static IReadOnlyList<Token> Lex(string srcCode)
    {
        var tokens = new List<Token>();
        var stream = new CharStream(srcCode, ['.']);

        while (stream.IsCharInStream())
        {
            if (stream.IsCharNumeric())
            {
                tokens.Add(ReadNumber(stream));
                continue;
            }

            stream.ReadNextChar();
        }

        return tokens;
    }

    static Token ReadNumber(CharStream stream)
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);

        while (stream.IsCharNumeric())
            builder.Append(stream.ReadNextChar());

        if (!stream.TryPeekChar(out char next) || next != '.')
            return builder.Build();

        // Consume the '.'
        stream.ReadNextChar();

        if (!stream.IsCharNumeric())
            throw new ArgumentException($"Invalid float literal '{builder}.' — expected digit after decimal point");

        builder.Append('.');
        builder.SetType(TokenType.FloatLiteral);

        while (stream.IsCharNumeric())
            builder.Append(stream.ReadNextChar());

        if (stream.TryPeekChar(out char extra) && extra == '.')
            throw new ArgumentException($"Invalid float literal '{builder}' — unexpected '.'");

        return builder.Build();
    }
}

struct Token
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

enum TokenType
{
    None,
    IntLiteral,
    FloatLiteral,
}