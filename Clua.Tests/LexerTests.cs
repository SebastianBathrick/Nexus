using Clua;
namespace Clua.Tests;

[TestFixture]
public class LexerTests
{
    // Identifier

    [Test]
    public void Lex_SimpleIdentifier_ReturnsIdentifierToken()
    {
        var tokens = Lexer.Lex("hello");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("hello"));
    }

    [Test]
    public void Lex_IdentifierStartingWithUnderscore_ReturnsIdentifierToken()
    {
        var tokens = Lexer.Lex("_hello");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("_hello"));
    }

    [Test]
    public void Lex_IdentifierWithUnderscoreOnly_ReturnsIdentifierToken()
    {
        var tokens = Lexer.Lex("_");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("_"));
    }

    [Test]
    public void Lex_IdentifierWithDigits_ReturnsIdentifierToken()
    {
        var tokens = Lexer.Lex("hello123");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("hello123"));
    }

    [Test]
    public void Lex_IdentifierWithUnderscoreAndDigits_ReturnsIdentifierToken()
    {
        var tokens = Lexer.Lex("_my_var_1");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("_my_var_1"));
    }

    [Test]
    public void Lex_DigitsThenAlpha_ProducesIntLiteralThenIdentifier()
    {
        var tokens = Lexer.Lex("123abc");
        Assert.That(tokens.Count, Is.EqualTo(2));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.IntLiteral));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("123"));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Plaintext, Is.EqualTo("abc"));
    }

    [Test]
    public void Lex_MultipleIdentifiers_ReturnsAllIdentifierTokens()
    {
        var tokens = Lexer.Lex("foo bar");
        var identifiers = tokens.Where(t => t.Type == TokenType.Identifier).ToList();
        Assert.That(identifiers.Count, Is.EqualTo(2));
        Assert.That(identifiers[0].Plaintext, Is.EqualTo("foo"));
        Assert.That(identifiers[1].Plaintext, Is.EqualTo("bar"));
    }

    // Keywords

    static IEnumerable<string> KeywordStrings => SyntaxSpecSheet.ReservedKeywords.Keys;

    [TestCaseSource(nameof(KeywordStrings))]
    public void Lex_ReservedKeyword_ReturnsKeywordToken(string keyword)
    {
        var expectedType = SyntaxSpecSheet.ReservedKeywords[keyword];
        var tokens = Lexer.Lex(keyword);
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
        Assert.That(tokens[0].Plaintext, Is.EqualTo(keyword));
    }

    [TestCaseSource(nameof(KeywordStrings))]
    public void Lex_KeywordPrefixedWithUnderscore_ReturnsIdentifier(string keyword)
    {
        var tokens = Lexer.Lex("_" + keyword);
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
    }

    [TestCaseSource(nameof(KeywordStrings))]
    public void Lex_KeywordWithTrailingAlpha_ReturnsIdentifier(string keyword)
    {
        var tokens = Lexer.Lex(keyword + "x");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Lex_MultipleKeywords_ReturnsCorrectTypes()
    {
        var keywords = SyntaxSpecSheet.ReservedKeywords.ToList();
        if (keywords.Count < 2) return;

        var input = string.Join(" ", keywords.Select(k => k.Key));
        var tokens = Lexer.Lex(input).ToList();

        Assert.That(tokens.Count, Is.EqualTo(keywords.Count));
        for (int i = 0; i < keywords.Count; i++)
            Assert.That(tokens[i].Type, Is.EqualTo(keywords[i].Value));
    }

    [TestCaseSource(nameof(KeywordStrings))]
    public void Lex_KeywordMixedWithIdentifier_ReturnsBothCorrectly(string keyword)
    {
        var expectedType = SyntaxSpecSheet.ReservedKeywords[keyword];
        var tokens = Lexer.Lex(keyword + " myVar");
        Assert.That(tokens.Count, Is.EqualTo(2));
        Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Plaintext, Is.EqualTo("myVar"));
    }
    // Number error cases

    [Test]
    public void Lex_TrailingDot_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Lexer.Lex("3."));
    }

    [Test]
    public void Lex_DoubleDotInFloat_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Lexer.Lex("3.14.5"));
    }

    // Invalid character

    [Test]
    public void Lex_InvalidCharacter_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Lexer.Lex("hello@world"));
    }
}
