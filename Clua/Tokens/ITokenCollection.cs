namespace Clua.Tokens
{
    interface ITokenCollection
    {
        public bool IsEmpty { get; }

        public void Add(Token token);

        public Token Read();

        public TokenType ReadType();

        public void Consume() => Read();

        public TokenType PeekType();

        public bool IsOfTypeAndConsume(TokenType tokenType);

        public bool IsOfType(TokenType tokenType);

        public bool IsOfType(params TokenType[] tokenTypes)
        {
            return tokenTypes.Any(IsOfType);
        }

        public IReadOnlyList<Token> ToList();
    }
}
