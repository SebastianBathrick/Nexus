namespace Clua;

class TokenCollection : ITokenCollection
{
    readonly List<Token> _tokens = [];

    public void Add(Token token)
    {
        _tokens.Add(token);
    }

    public IReadOnlyList<Token> Tokens => _tokens;
}
