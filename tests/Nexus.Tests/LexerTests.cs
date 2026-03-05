using Nexus;
using Nexus.LexicalAnalysis;
using Nexus.LexicalAnalysis;

namespace Nexus.Tests
{
    /// <summary>Stub used only by LexerTests: implements Add and ToList so the Lexer can produce a list of tokens. Parser/integration tests use Nexus.Tokens.TokenCollection.</summary>
    class LexerTestTokenCollection : ITokenCollection
    {
        readonly List<Token> _tokens = [];

        public bool IsEmpty { get; }

        public void Add(Token token) => _tokens.Add(token);

        public Token Read() => throw new NotImplementedException();

        public TokenType ReadType() => throw new NotImplementedException();

        public TokenType PeekType() => throw new NotImplementedException();

        public bool IsOfTypeAndConsume(TokenType tokenType) => throw new NotImplementedException();

        public bool IsOfType(TokenType tokenType) => throw new NotImplementedException();

        public IReadOnlyList<Token> ToList() => new List<Token>(_tokens);
    }

    [TestFixture]
    public class LexerTests
    {
        // Identifier

        [Test]
        public void Lex_SimpleIdentifier_ReturnsIdentifierToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("hello").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("hello"));
        }

        [Test]
        public void Lex_IdentifierStartingWithUnderscore_ReturnsIdentifierToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("_hello").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("_hello"));
        }

        [Test]
        public void Lex_IdentifierWithUnderscoreOnly_ReturnsIdentifierToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("_").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("_"));
        }

        [Test]
        public void Lex_IdentifierWithDigits_ReturnsIdentifierToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("hello123").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("hello123"));
        }

        [Test]
        public void Lex_IdentifierWithUnderscoreAndDigits_ReturnsIdentifierToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("_my_var_1").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("_my_var_1"));
        }

        [Test]
        public void Lex_DigitsThenAlpha_ProducesIntLiteralThenIdentifier()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("123abc").ToList();
            Assert.That(tokens.Count, Is.EqualTo(2));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.NumberLiteral));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("123"));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[1].Plaintext, Is.EqualTo("abc"));
        }

        [Test]
        public void Lex_MultipleIdentifiers_ReturnsAllIdentifierTokens()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("foo bar").ToList();
            var identifiers = tokens.Where(t => t.Type == TokenType.Identifier).ToList();
            Assert.That(identifiers.Count, Is.EqualTo(2));
            Assert.That(identifiers[0].Plaintext, Is.EqualTo("foo"));
            Assert.That(identifiers[1].Plaintext, Is.EqualTo("bar"));
        }

        // Keywords

        static IEnumerable<string> KeywordStrings => Nexus.LanguageSpecifications.ReservedKeywords.Keys;

        [TestCaseSource(nameof(KeywordStrings))]
        public void Lex_ReservedKeyword_ReturnsKeywordToken(string keyword)
        {
            var expectedType = Nexus.LanguageSpecifications.ReservedKeywords[keyword];
            var tokens = Lexer.Lex<LexerTestTokenCollection>(keyword).ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
            Assert.That(tokens[0].Plaintext, Is.EqualTo(keyword));
        }

        [TestCaseSource(nameof(KeywordStrings))]
        public void Lex_KeywordPrefixedWithUnderscore_ReturnsIdentifier(string keyword)
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("_" + keyword).ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        }

        [TestCaseSource(nameof(KeywordStrings))]
        public void Lex_KeywordWithTrailingAlpha_ReturnsIdentifier(string keyword)
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>(keyword + "x").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
        }

        [Test]
        public void Lex_MultipleKeywords_ReturnsCorrectTypes()
        {
            var keywords = Nexus.LanguageSpecifications.ReservedKeywords.ToList();
            if (keywords.Count < 2) return;

            var input = string.Join(" ", keywords.Select(k => k.Key));
            var tokens = Lexer.Lex<LexerTestTokenCollection>(input).ToList();

            Assert.That(tokens.Count, Is.EqualTo(keywords.Count));
            for (int i = 0; i < keywords.Count; i++)
                Assert.That(tokens[i].Type, Is.EqualTo(keywords[i].Value));
        }

        [TestCaseSource(nameof(KeywordStrings))]
        public void Lex_KeywordMixedWithIdentifier_ReturnsBothCorrectly(string keyword)
        {
            var expectedType = Nexus.LanguageSpecifications.ReservedKeywords[keyword];
            var tokens = Lexer.Lex<LexerTestTokenCollection>(keyword + " myVar").ToList();
            Assert.That(tokens.Count, Is.EqualTo(2));
            Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[1].Plaintext, Is.EqualTo("myVar"));
        }

        // Operators

        static IEnumerable<string> OperatorStrings => Nexus.LanguageSpecifications.Operators.Keys;

        [TestCaseSource(nameof(OperatorStrings))]
        public void Lex_ValidOperator_ReturnsCorrectOperatorToken(string op)
        {
            var expectedType = Nexus.LanguageSpecifications.Operators[op];
            var tokens = Lexer.Lex<LexerTestTokenCollection>(op).ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(expectedType));
        }

        [Test]
        public void Lex_ValidOperatorSequence_ReturnsSeparateMinusToken()
        {
            var tkns = Lexer.Lex<LexerTestTokenCollection>("a+-b").ToList();
            Assert.That(tkns[1].Type, Is.EqualTo(TokenType.SymbolPlus));
            Assert.That(tkns[2].Type, Is.EqualTo(TokenType.SymbolMinus));
        }

        [Test]
        public void Lex_OperatorBetweenIdentifiers_ReturnsThreeTokens()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("a+b").ToList();
            Assert.That(tokens.Count, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.SymbolPlus));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Identifier));
        }

        [Test]
        public void Lex_OperatorBetweenIntLiterals_ReturnsThreeTokens()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("1+2").ToList();
            Assert.That(tokens.Count, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.NumberLiteral));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.SymbolPlus));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.NumberLiteral));
        }

        [Test]
        public void Lex_AssignEquals_ReturnsEqualsOperator()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("a=b").ToList();
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.KeywordEquals));
        }

        [Test]
        public void Lex_DoubleEquals_ReturnsEqualityOperator()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("a==b").ToList();
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.KeywordEquals));
        }

        // Delimiters

        [Test]
        public void Lex_OpenParen_ReturnsOpenParenToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("(").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.DelimiterOpenParen));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("("));
        }

        [Test]
        public void Lex_CloseParen_ReturnsCloseParenToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>(")").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.DelimiterCloseParen));
            Assert.That(tokens[0].Plaintext, Is.EqualTo(")"));
        }

        [Test]
        public void Lex_ParenWrappedIdentifier_ReturnsThreeTokens()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("(a)").ToList();
            Assert.That(tokens.Count, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.DelimiterOpenParen));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.DelimiterCloseParen));
        }

        [Test]
        public void Lex_CurlyOpen_ReturnsCurlyOpenToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("{").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.DelimiterCurlyOpen));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("{"));
        }

        [Test]
        public void Lex_CurlyClose_ReturnsCurlyCloseToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("}").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.DelimiterCurlyClose));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("}"));
        }

        [Test]
        public void Lex_CurlyWrappedIdentifier_ReturnsThreeTokens()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("{a}").ToList();
            Assert.That(tokens.Count, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.DelimiterCurlyOpen));
            Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Identifier));
            Assert.That(tokens[2].Type, Is.EqualTo(TokenType.DelimiterCurlyClose));
        }

        // Number literals

        [Test]
        public void Lex_IntegerLiteral_ReturnsSingleNumberLiteralToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("42").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.NumberLiteral));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("42"));
        }

        [Test]
        public void Lex_DecimalLiteral_ReturnsNumberLiteralWithDecimal()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("3.14").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.NumberLiteral));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("3.14"));
        }

        [Test]
        public void Lex_Zero_ReturnsNumberLiteralToken()
        {
            var tokens = Lexer.Lex<LexerTestTokenCollection>("0").ToList();
            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(TokenType.NumberLiteral));
            Assert.That(tokens[0].Plaintext, Is.EqualTo("0"));
        }

        // Number error cases

        [Test]
        public void Lex_TrailingDot_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Lexer.Lex<LexerTestTokenCollection>("3."));
        }

        [Test]
        public void Lex_DoubleDotInFloat_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Lexer.Lex<LexerTestTokenCollection>("3.14.5"));
        }

        // Invalid character

        [Test]
        public void Lex_InvalidCharacter_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Lexer.Lex<LexerTestTokenCollection>("hello@world"));
        }
    }
}
