namespace Nexus.LexicalAnalysis
{
    readonly struct Token
    {
        readonly string? _plaintext;

        // Return empty string each time, because the class should only retrieve/store the plaintext of specific token types
        public string Plaintext => _plaintext ?? string.Empty;

        public TokenType Type { get; }

        public Token(TokenType type, string? plaintext = null)
        {
            Type = type;
            _plaintext = plaintext;
        }
    }
}
