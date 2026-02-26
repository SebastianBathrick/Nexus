namespace Cemble.Tests;

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
    /*
    [TestCase("float", TokenType.FloatKeyword)]
    [TestCase("int", TokenType.IntKeyword)]
    [TestCase("string", TokenType.StringKeyword)]
    [TestCase("bool", TokenType.BoolKeyword)]
    [TestCase("true", TokenType.TrueKeyword)]
    [TestCase("false", TokenType.FalseKeyword)]
    public void Lex_ReservedKeyword_ReturnsKeywordToken(string keyword, TokenType expectedType)
    {
        var tokens = Lexer.Lex(keyword);
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
        Assert.That(tokens[0].Plaintext, Is.EqualTo(keyword));
    }   


    [Test]
    public void Lex_KeywordPrefixedWithUnderscore_ReturnsIdentifier()
    {
        var tokens = Lexer.Lex("_int");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Lex_KeywordWithTrailingChars_ReturnsIdentifier()
    {
        var tokens = Lexer.Lex("integer");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Lex_MultipleKeywords_ReturnsAllKeywordTokens()
    {
        var tokens = Lexer.Lex("int float");
        Assert.That(tokens.Count, Is.EqualTo(2));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.IntKeyword));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.FloatKeyword));
    }

    [Test]
    public void Lex_KeywordMixedWithIdentifier_ReturnsBothCorrectly()
    {
        var tokens = Lexer.Lex("int myVar");
        Assert.That(tokens.Count, Is.EqualTo(2));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.IntKeyword));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Plaintext, Is.EqualTo("myVar"));
    }
    */
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
