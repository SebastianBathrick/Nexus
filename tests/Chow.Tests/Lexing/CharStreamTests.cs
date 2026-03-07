using Chow.Lexing;

namespace Chow.Tests;

[TestFixture]
public class CharStreamTests
{
    // Construction

    [Test]
    public void Constructor_ValidAlphanumericAndWhitespace_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new CharStream("hello world 123"));
    }

    [Test]
    public void Constructor_EmptyString_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new CharStream(""));
    }

    [Test]
    public void Constructor_InvalidCharacter_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CharStream("hello@"));
    }

    [TestCase("hello@")]
    [TestCase("123#")]
    [TestCase("@abc")]
    [TestCase("abc$def")]
    public void Constructor_InvalidCharacters_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => new CharStream(input));
    }

    [Test]
    public void Constructor_InvalidCharacterAtIndexZero_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => new CharStream(CharStream.InvalidChar + "abc"));
        Assert.That(ex!.Message, Does.Contain(CharStream.InvalidChar));
        Assert.That(ex.Message, Does.Contain("0"));
    }

    [Test]
    public void Constructor_InvalidCharacter_ErrorMessageContainsCharacterAndIndex()
    {
        var ex = Assert.Throws<ArgumentException>(() => new CharStream("abc@def"));
        Assert.That(ex!.Message, Does.Contain("@"));
        Assert.That(ex.Message, Does.Contain("3"));
    }

    [Test]
    public void Constructor_UnderscoreAndDecimal_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new CharStream("_hello 3.14"));
    }

    [Test]
    public void Constructor_ValidOperatorCharacters_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new CharStream("+ - * / = ! < > & |"));
    }

    // IsCharInStream

    [Test]
    public void IsCharInStream_NonEmptyStream_ReturnsTrue()
    {
        var stream = new CharStream("a");
        Assert.That(stream.IsCharInStream(), Is.True);
    }

    [Test]
    public void IsCharInStream_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharInStream(), Is.False);
    }

    [Test]
    public void IsCharInStream_AfterExhausting_ReturnsFalse()
    {
        var stream = new CharStream("ab");
        stream.ReadNextChar();
        stream.ReadNextChar();
        Assert.That(stream.IsCharInStream(), Is.False);
    }

    // ReadNextChar

    [Test]
    public void ReadNextChar_ReturnsCharsInOrder()
    {
        var stream = new CharStream("abc");
        Assert.That(stream.ReadNextChar(), Is.EqualTo('a'));
        Assert.That(stream.ReadNextChar(), Is.EqualTo('b'));
        Assert.That(stream.ReadNextChar(), Is.EqualTo('c'));
    }

    [Test]
    public void ReadNextChar_WhenExhausted_ReturnsNullChar()
    {
        var stream = new CharStream("a");
        stream.ReadNextChar();
        Assert.That(stream.ReadNextChar(), Is.EqualTo('\0'));
    }

    [Test]
    public void ReadNextChar_EmptyStream_ReturnsNullChar()
    {
        var stream = new CharStream("");
        Assert.That(stream.ReadNextChar(), Is.EqualTo('\0'));
    }

    // TryPeekChar

    [Test]
    public void TryPeekChar_NonEmptyStream_ReturnsTrueAndCurrentChar()
    {
        var stream = new CharStream("abc");
        var result = stream.TryPeekChar(out var c);
        Assert.That(result, Is.True);
        Assert.That(c, Is.EqualTo('a'));
    }

    [Test]
    public void TryPeekChar_DoesNotAdvancePosition()
    {
        var stream = new CharStream("abc");
        stream.TryPeekChar(out _);
        stream.TryPeekChar(out var c);
        Assert.That(c, Is.EqualTo('a'));
    }

    [Test]
    public void TryPeekChar_EmptyStream_ReturnsFalseAndNullChar()
    {
        var stream = new CharStream("");
        var result = stream.TryPeekChar(out var c);
        Assert.That(result, Is.False);
        Assert.That(c, Is.EqualTo('\0'));
    }

    [Test]
    public void TryPeekChar_WhenExhausted_ReturnsFalse()
    {
        var stream = new CharStream("a");
        stream.ReadNextChar();
        Assert.That(stream.TryPeekChar(out _), Is.False);
    }

    // IsCharNumeric

    [TestCase("0")]
    [TestCase("5")]
    [TestCase("9")]
    public void IsCharNumeric_NumericChar_ReturnsTrue(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharNumeric(), Is.True);
    }

    [TestCase("a")]
    [TestCase("Z")]
    [TestCase(" ")]
    public void IsCharNumeric_NonNumericChar_ReturnsFalse(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharNumeric(), Is.False);
    }

    [Test]
    public void IsCharNumeric_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharNumeric(), Is.False);
    }

    // IsCharAlpha

    [TestCase("a")]
    [TestCase("z")]
    [TestCase("A")]
    [TestCase("Z")]
    [TestCase("m")]
    public void IsCharAlpha_AlphaChar_ReturnsTrue(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharAlpha(), Is.True);
    }

    [TestCase("0")]
    [TestCase("9")]
    [TestCase(" ")]
    [TestCase("_")]
    public void IsCharAlpha_NonAlphaChar_ReturnsFalse(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharAlpha(), Is.False);
    }

    [Test]
    public void IsCharAlpha_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharAlpha(), Is.False);
    }

    // IsCharWhitespace

    [TestCase(" ")]
    [TestCase("\t")]
    [TestCase("\n")]
    [TestCase("\r")]
    [TestCase("\v")]
    [TestCase("\f")]
    public void IsCharWhitespace_WhitespaceChar_ReturnsTrue(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharWhitespace(), Is.True);
    }

    [TestCase("a")]
    [TestCase("1")]
    public void IsCharWhitespace_NonWhitespaceChar_ReturnsFalse(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharWhitespace(), Is.False);
    }

    [Test]
    public void IsCharWhitespace_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharWhitespace(), Is.False);
    }

    // IsCharUnderscore

    [Test]
    public void IsCharUnderscore_Underscore_ReturnsTrue()
    {
        var stream = new CharStream("_");
        Assert.That(stream.IsCharUnderscore(), Is.True);
    }

    [TestCase("a")]
    [TestCase("1")]
    [TestCase(" ")]
    public void IsCharUnderscore_NonUnderscore_ReturnsFalse(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharUnderscore(), Is.False);
    }

    [Test]
    public void IsCharUnderscore_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharUnderscore(), Is.False);
    }

    // IsCharDecimal

    [Test]
    public void IsCharDecimal_Dot_ReturnsTrue()
    {
        var stream = new CharStream(".");
        Assert.That(stream.IsCharDot(), Is.True);
    }

    [TestCase("a")]
    [TestCase("1")]
    [TestCase(" ")]
    [TestCase("_")]
    public void IsCharDecimal_NonDot_ReturnsFalse(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharDot(), Is.False);
    }

    [Test]
    public void IsCharDecimal_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharDot(), Is.False);
    }

    // IsCharOperator

    [TestCase("+")]
    [TestCase("-")]
    [TestCase("*")]
    [TestCase("/")]
    [TestCase("=")]
    [TestCase("!")]
    [TestCase("<")]
    [TestCase(">")]
    [TestCase("&")]
    [TestCase("|")]
    public void IsCharOperator_OperatorChar_ReturnsTrue(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharOperator(), Is.True);
    }

    [TestCase("a")]
    [TestCase("1")]
    [TestCase(" ")]
    [TestCase("_")]
    [TestCase(".")]
    public void IsCharOperator_NonOperatorChar_ReturnsFalse(string input)
    {
        var stream = new CharStream(input);
        Assert.That(stream.IsCharOperator(), Is.False);
    }

    [Test]
    public void IsCharOperator_EmptyStream_ReturnsFalse()
    {
        var stream = new CharStream("");
        Assert.That(stream.IsCharOperator(), Is.False);
    }
}
