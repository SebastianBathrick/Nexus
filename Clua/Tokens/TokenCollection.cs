namespace Clua.Tokens;

class TokenCollection : ITokenCollection
{
    readonly List<Token> _tokens = [];

    public IReadOnlyList<Token> Tokens => _tokens;

    #region ITokenCollection Members

    public void Add(Token token)
    {
        _tokens.Add(token);
    }

    #endregion
}
