namespace Clua.Tokens;

interface ITokenCollection
{
    public bool IsEmpty { get; }
    
    public void Add(Token token);
    
    public bool IsMatchAndConsumed(TokenType tokenType);

    public bool IsOfType(TokenType tokenType);
    
    public IReadOnlyList<Token> ToList();
}
