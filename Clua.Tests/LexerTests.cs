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

    static IEnumerable<string> KeywordStrings => LanguageSpecifications.ReservedKeywords.Keys;

    [TestCaseSource(nameof(KeywordStrings))]
    public void Lex_ReservedKeyword_ReturnsKeywordToken(string keyword)
    {
        var expectedType = LanguageSpecifications.ReservedKeywords[keyword];
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
        var keywords = LanguageSpecifications.ReservedKeywords.ToList();
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
        var expectedType = LanguageSpecifications.ReservedKeywords[keyword];
        var tokens = Lexer.Lex(keyword + " myVar");
        Assert.That(tokens.Count, Is.EqualTo(2));
        Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Plaintext, Is.EqualTo("myVar"));
    }
    // Operators

    static IEnumerable<string> OperatorStrings => LanguageSpecifications.ValidOperators.Keys;

    [TestCaseSource(nameof(OperatorStrings))]
    public void Lex_ValidOperator_ReturnsCorrectOperatorToken(string op)
    {
        var expectedType = LanguageSpecifications.ValidOperators[op];
        var tokens = Lexer.Lex(op);
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
    }

    [Test]
    public void Lex_InvalidOperatorSequence_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Lexer.Lex("+-"));
    }

    [Test]
    public void Lex_OperatorBetweenIdentifiers_ReturnsThreeTokens()
    {
        var tokens = Lexer.Lex("a+b");
        Assert.That(tokens.Count, Is.EqualTo(3));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.PlusOperator));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
    }

    [Test]
    public void Lex_OperatorBetweenIntLiterals_ReturnsThreeTokens()
    {
        var tokens = Lexer.Lex("1+2");
        Assert.That(tokens.Count, Is.EqualTo(3));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.IntLiteral));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.PlusOperator));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.IntLiteral));
    }

    [Test]
    public void Lex_AssignEquals_ReturnsEqualsOperator()
    {
        var tokens = Lexer.Lex("a=b");
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EqualsOperator));
    }

    [Test]
    public void Lex_DoubleEquals_ReturnsEqualityOperator()
    {
        var tokens = Lexer.Lex("a==b");
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.EqualityOperator));
    }

    // Parentheses

    [Test]
    public void Lex_OpenParen_ReturnsOpenParenToken()
    {
        var tokens = Lexer.Lex("(");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[0].Plaintext, Is.EqualTo("("));
    }

    [Test]
    public void Lex_CloseParen_ReturnsCloseParenToken()
    {
        var tokens = Lexer.Lex(")");
        Assert.That(tokens.Count, Is.EqualTo(1));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.CloseParen));
        Assert.That(tokens[0].Plaintext, Is.EqualTo(")"));
    }

    [Test]
    public void Lex_ParenWrappedIdentifier_ReturnsThreeTokens()
    {
        var tokens = Lexer.Lex("(a)");
        Assert.That(tokens.Count, Is.EqualTo(3));
        Assert.That(tokens[0].Type, Is.EqualTo(TokenType.OpenParen));
        Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
        Assert.That(tokens[2].Type, Is.EqualTo(TokenType.CloseParen));
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
