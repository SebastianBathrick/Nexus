using System.Text;

namespace Clua;

class TokenBuilder
{
    private StringBuilder? _builder;
    private TokenType _tokenType;
    
    public bool IsValid => _tokenType != TokenType.None;
    
    public TokenBuilder(TokenType tokenType = TokenType.None)
    {
        _builder = null;
        _tokenType = tokenType;
    }
    
    public void SetType(TokenType newType)
    {
        _tokenType = newType;
    }
    
    public void Append(char c)
    {
        _builder ??= new();
        _builder.Append(c);
    }
    
    public bool TryBuild(out Token token, TokenType tokenType = TokenType.None)
    {
        if (!IsValid)
        {
            token = default;
            return false;
        }

        token = new Token(_tokenType, _builder?.ToString());
        _builder = null;
        _tokenType = TokenType.None;
        return true;
    }

    public override string ToString() => _builder?.ToString() ?? string.Empty;

    public Token Build(TokenType tokenType = TokenType.None)
    {
        if (IsValid && tokenType != TokenType.None)
            throw new InvalidOperationException("Cannot pass a token type when one is already set — use ChangeType instead");

        if (!IsValid && tokenType == TokenType.None)
            throw new InvalidOperationException("Cannot build a token with type None");

        var token = new Token(IsValid ? _tokenType : tokenType, _builder?.ToString());
        _builder = null;
        _tokenType = TokenType.None;
        return token;
    }
}