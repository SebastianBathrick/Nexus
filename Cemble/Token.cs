namespace Cemble;

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
