namespace Clua.Tokens;

interface ITokenCollection
{
    public void Add(Token token);

    public IReadOnlyList<Token> ToList();
}
