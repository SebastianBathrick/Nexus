using Chow.Logging;

namespace Chow.Tests;

[TestFixture]
public class LoggerInsertPropertiesTests
{
    [TestCase("hello", "hello")]
    [TestCase("", "")]
    [TestCase("hello world", "hello world")]
    [TestCase("no braces here", "no braces here")]
    public void InsertProperties_NoPlaceholders_ReturnsOriginalMessage(string msg, string expected)
    {
        var result = LoggerTestHelper.InsertProperties(msg);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{x}", "a", "a")]
    [TestCase("{0}", "value", "value")]
    [TestCase("{tag}", "replaced", "replaced")]
    [TestCase("pre {tag} post", "pre mid post", "mid")]
    [TestCase("{a} {b}", "1 2", "1", "2")]
    [TestCase("{first} and {second}", "hello and world", "hello", "world")]
    [TestCase("{a}{b}{c}", "123", "1", "2", "3")]
    public void InsertProperties_SubstitutesPlaceholders(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{x} and {x}", "val and val", "val")]
    [TestCase("{a} {b} {a}", "1 2 1", "1", "2")]
    [TestCase("{name} is {name}", "Alice is Alice", "Alice")]
    [TestCase("{x}{x}{x}", "aaa", "a")]
    [TestCase("Error in {Method}: {Method} failed", "Error in DoWork: DoWork failed", "DoWork")]
    public void InsertProperties_SameTagMultipleTimes_ReusesFirstBoundProp(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("hello {world", "hello {world")]
    [TestCase("unclosed {brace", "unclosed {brace")]
    [TestCase("{", "{")]
    [TestCase("text { more text", "text { more text")]
    public void InsertProperties_UnclosedBrace_LeavesLiteral(string msg, string expected)
    {
        var result = LoggerTestHelper.InsertProperties(msg);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{}", "value", "value")]
    [TestCase("{} and {}", "1 and 1", "1", "2")]
    public void InsertProperties_EmptyTag_SubstitutesWhenPropsAvailable(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{}", "{}")]
    [TestCase("{} {}", "{} {}")]
    public void InsertProperties_EmptyTag_LeavesPlaceholderWhenNoProps(string msg, string expected)
    {
        var result = LoggerTestHelper.InsertProperties(msg);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{a}{b}", "12", "1", "2")]
    [TestCase("{x}{y}{z}", "abc", "a", "b", "c")]
    public void InsertProperties_AdjacentPlaceholders_SubstitutesWithoutSpaces(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{x}", "v", "v")]
    [TestCase("{a}{b}", "12", "1", "2")]
    public void InsertProperties_OnlyPlaceholders_SubstitutesAll(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{a} {b}", "x {b}", "x")]
    [TestCase("{first} {second} {third}", "1 2 {third}", "1", "2")]
    [TestCase("{a} {b} {c}", "1 {b} {c}", "1")]
    public void InsertProperties_MorePlaceholdersThanProps_LeavesUnmatchedAsIs(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("User {UserId} logged in from {IP}", "User 42 logged in from 10.0.0.1", 42, "10.0.0.1")]
    [TestCase("Processing {Count} items in {Duration}ms", "Processing 100 items in 250ms", 100, 250)]
    [TestCase("{Timestamp}: {Level} - {Message}", "2024-01-01: Info - Started", "2024-01-01", "Info", "Started")]
    public void InsertProperties_TypicalLogMessages_SubstitutesCorrectly(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(NullPropsTestCases))]
    public void InsertProperties_NullProps_LeavesPlaceholdersAsIs(string msg, object[]? props, string expected)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    static IEnumerable<TestCaseData> NullPropsTestCases()
    {
        yield return new TestCaseData("hello {x}", null, "hello {x}")
            .SetName("NullPropsArray_LeavesPlaceholder");

        yield return new TestCaseData("{name}", null, "{name}")
            .SetName("NullPropsArray_SinglePlaceholder");

        yield return new TestCaseData("{a} {b} {c}", null, "{a} {b} {c}")
            .SetName("NullPropsArray_MultiplePlaceholders");
    }

    [TestCaseSource(nameof(EmptyPropsTestCases))]
    public void InsertProperties_EmptyPropsArray_LeavesPlaceholdersAsIs(string msg, object[]? props, string expected)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    static IEnumerable<TestCaseData> EmptyPropsTestCases()
    {
        yield return new TestCaseData("hello {x}", new object[] { }, "hello {x}")
            .SetName("EmptyPropsArray_LeavesPlaceholder");

        yield return new TestCaseData("{tag}", new object[] { }, "{tag}")
            .SetName("EmptyPropsArray_SinglePlaceholder");
    }

    [TestCaseSource(nameof(NullPropValueTestCases))]
    public void InsertProperties_NullPropValue_RendersAsNull(string msg, object[]? props, string expected)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    static IEnumerable<TestCaseData> NullPropValueTestCases()
    {
        yield return new TestCaseData("{x}", new object?[] { null }, ILogger.NullPropertyLiteral)
            .SetName("NullPropValue_SinglePlaceholder");

        yield return new TestCaseData("{a} {b}", new object?[] { "value", null }, $"value {ILogger.NullPropertyLiteral}")
            .SetName("NullPropValue_SecondPropNull");

        yield return new TestCaseData("{first} {second} {third}", new object?[] { null, "mid", null }, $"{ILogger.NullPropertyLiteral} mid {ILogger.NullPropertyLiteral}")
            .SetName("NullPropValue_FirstAndThirdNull");
    }

    [Test]
    public void InsertProperties_PropWithToStringReturningNull_RendersAsNull()
    {
        var customObj = new CustomTypeWithNullToString();
        var result = LoggerTestHelper.InsertProperties("{value}", customObj);
        Assert.That(result, Is.EqualTo(ILogger.NullPropertyLiteral));
    }

    [Test]
    public void InsertProperties_MixedTypes_ConvertsAllToString()
    {
        var result = LoggerTestHelper.InsertProperties("{int} {double} {bool} {char}", 42, 3.14, true, 'A');
        Assert.That(result, Is.EqualTo("42 3.14 True A"));
    }

    [Test]
    public void InsertProperties_ComplexObject_UsesToString()
    {
        var obj = new CustomTypeWithToString("TestValue");
        var result = LoggerTestHelper.InsertProperties("Object: {obj}", obj);
        Assert.That(result, Is.EqualTo("Object: CustomTypeWithToString: TestValue"));
    }

    [TestCase("{a} and {a} and {a}", "x and x and x", "x")]
    [TestCase("{tag} {tag} {tag} {tag}", "val val val val", "val")]
    public void InsertProperties_SameTagManyTimes_AlwaysUsesFirstBoundValue(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("{{escaped}}", "{{escaped}}")]
    [TestCase("{normal}", "value", "value")]
    public void InsertProperties_VariousBracePatterns(string msg, string expected, params object[] props)
    {
        var result = LoggerTestHelper.InsertProperties(msg, props);
        Assert.That(result, Is.EqualTo(expected));
    }
}

class LoggerTestHelper : Logger
{
    public new static string InsertProperties(string msg, params object[]? props)
        => new LoggerTestHelper().InsertPropertiesPublic(msg, props);

    string InsertPropertiesPublic(string msg, params object[]? props)
        => base.InsertProperties(msg, props);

    protected override void OutputFormattedMessage(string msg)
    {
    }
}

class CustomTypeWithNullToString
{
    public override string? ToString() => null;
}

class CustomTypeWithToString
{
    readonly string _value;

    public CustomTypeWithToString(string value)
    {
        _value = value;
    }

    public override string ToString() => $"CustomTypeWithToString: {_value}";
}
