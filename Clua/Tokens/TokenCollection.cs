namespace Clua.Tokens;

class TokenCollection : ITokenCollection
{
    readonly List<Token> _tokens = [];
    int _index;

    public IReadOnlyList<Token> Tokens => _tokens;

    #region ITokenCollection Members

    public bool IsEmpty => _index >= _tokens.Count;

    public void Add(Token token)
    {
        _tokens.Add(token);
    }

    public bool IsMatchAndConsumed(TokenType tokenType)
    {
        if (_index >= _tokens.Count || _tokens[_index].Type != tokenType)
            return false;

        _index++;
        return true;
    }

    public bool IsOfType(TokenType tokenType)
    {
        return _index < _tokens.Count && _tokens[_index].Type == tokenType;
    }

    public IReadOnlyList<Token> ToList()
    {
        // Ensure to copy the list to prevent external modification of the internal list
        return new List<Token>(_tokens);
    }

    #endregion
}
