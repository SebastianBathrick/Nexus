namespace Clua;

class Lexer
{
    public static IReadOnlyList<Token> Lex(string srcCode)
    {
        var tknList = new List<Token>();
        var stream = new CharStream(srcCode);

        while (stream.IsCharInStream())
        {
            Token tkn;
            
            switch (stream.GetCharType())
            {
                case CharType.Numeric: 
                    tkn = ReadNumber(stream); 
                    break;
                case CharType.Operator:
                    tkn = ReadOperator(stream);
                    break;
                case CharType.Alpha: 
                case CharType.Underscore: 
                    tkn = ReadIdentifier(stream); 
                    break;
                case CharType.Whitespace:
                    stream.IgnoreChar();
                    continue;
                case CharType.Invalid: default:
                    throw new ArgumentException($"Invalid character '{stream.ReadNextChar()}'");
            }
            
            tknList.Add(tkn);
        }

        return tknList;
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

    static Token ReadOperator(CharStream stream)
    {
        var builder = new TokenBuilder();
        
        while (stream.IsCharOperator())
            builder.Append(stream.ReadNextChar());
        
        if (!SyntaxSpecSheet.ValidOperators.TryGetValue(builder.ToString(), out var operatorType))
            throw new ArgumentException($"Invalid operator '{builder}'");
            
        builder.SetType(operatorType);
        return builder.Build();
    }

    static Token ReadNumber(CharStream stream)
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);

        while (stream.IsCharNumeric())
            builder.Append(stream.ReadNextChar());
        
        // If there wasn't a dot return an integer literal token
        if (!stream.IsCharDot())
            return builder.Build();

        // Consume the '.'
        stream.IgnoreChar();

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