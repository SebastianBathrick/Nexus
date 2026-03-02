namespace Clua;

class Lexer
{
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

        if (SyntaxSpecSheet.ReservedKeywords.TryGetValue(builder.ToString(), out TokenType keywordType))
            builder.SetType(keywordType);

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