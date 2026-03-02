namespace Clua;

readonly struct Token(TokenType type, string? plaintext = null)
{
    // Return empty string each time, because the class should only retrieve/store the plaintext of specific token types
    public string Plaintext
    {
        get => field ?? string.Empty;
    } = plaintext;

    public TokenType Type { get; init; } = type;
}
