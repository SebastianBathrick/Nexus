using Clua;

namespace Clua.Tests;

[TestFixture]
public class TokenBuilderTests
{
    // IsValid

    [Test]
    public void IsValid_DefaultConstructor_ReturnsFalse()
    {
        var builder = new TokenBuilder();
        Assert.That(builder.IsValid, Is.False);
    }

    [Test]
    public void IsValid_ConstructedWithNone_ReturnsFalse()
    {
        var builder = new TokenBuilder(TokenType.None);
        Assert.That(builder.IsValid, Is.False);
    }

    [Test]
    public void IsValid_ConstructedWithValidType_ReturnsTrue()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        Assert.That(builder.IsValid, Is.True);
    }

    // ChangeType

    [Test]
    public void ChangeType_SetsTypeCorrectly()
    {
        var builder = new TokenBuilder();
        builder.SetType(TokenType.IntLiteral);
        Assert.That(builder.IsValid, Is.True);
    }

    [Test]
    public void ChangeType_ToNone_MakesIsValidFalse()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        builder.SetType(TokenType.None);
        Assert.That(builder.IsValid, Is.False);
    }

    // Build

    [Test]
    public void Build_WithValidType_ReturnsTokenWithCorrectType()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        builder.Append('1');
        var token = builder.Build();
        Assert.That(token.Type, Is.EqualTo(TokenType.IntLiteral));
    }

    [Test]
    public void Build_WithAppendedChars_ReturnsTokenWithCorrectPlaintext()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        builder.Append('1');
        builder.Append('2');
        builder.Append('3');
        var token = builder.Build();
        Assert.That(token.Plaintext, Is.EqualTo("123"));
    }

    [Test]
    public void Build_WithNoAppendedChars_ReturnsTokenWithEmptyPlaintext()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        var token = builder.Build();
        Assert.That(token.Plaintext, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Build_ResetsTypeToNone()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        builder.Build();
        Assert.That(builder.IsValid, Is.False);
    }

    [Test]
    public void Build_WhenTypeIsNone_ThrowsInvalidOperationException()
    {
        var builder = new TokenBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_WithTypeArg_WhenInternalTypeIsNone_ReturnsTokenWithArgType()
    {
        var builder = new TokenBuilder();
        builder.Append('1');
        var token = builder.Build(TokenType.IntLiteral);
        Assert.That(token.Type, Is.EqualTo(TokenType.IntLiteral));
    }

    [Test]
    public void Build_WithTypeArg_WhenInternalTypeIsAlreadySet_ThrowsInvalidOperationException()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        Assert.Throws<InvalidOperationException>(() => builder.Build(TokenType.FloatLiteral));
    }

    [Test]
    public void Build_WithSameTypeArg_WhenInternalTypeIsAlreadySet_ThrowsInvalidOperationException()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        Assert.Throws<InvalidOperationException>(() => builder.Build(TokenType.IntLiteral));
    }

    [Test]
    public void Build_BothNone_ThrowsInvalidOperationException()
    {
        var builder = new TokenBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.Build(TokenType.None));
    }

    // TryBuild

    [Test]
    public void TryBuild_WithValidType_ReturnsTrueAndCorrectToken()
    {
        var builder = new TokenBuilder(TokenType.FloatLiteral);
        builder.Append('3');
        builder.Append('.');
        builder.Append('1');
        var result = builder.TryBuild(out var token);
        Assert.That(result, Is.True);
        Assert.That(token.Type, Is.EqualTo(TokenType.FloatLiteral));
        Assert.That(token.Plaintext, Is.EqualTo("3.1"));
    }

    [Test]
    public void TryBuild_WhenTypeIsNone_ReturnsFalse()
    {
        var builder = new TokenBuilder();
        var result = builder.TryBuild(out var token);
        Assert.That(result, Is.False);
        Assert.That(token, Is.EqualTo(default(Token)));
    }

    [Test]
    public void TryBuild_ResetsStateAfterSuccess()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        builder.Append('1');
        builder.TryBuild(out _);
        Assert.That(builder.IsValid, Is.False);
    }

    [Test]
    public void TryBuild_CalledTwice_SecondReturnsFalse()
    {
        var builder = new TokenBuilder(TokenType.IntLiteral);
        builder.Append('1');
        builder.TryBuild(out _);
        var result = builder.TryBuild(out _);
        Assert.That(result, Is.False);
    }
}
