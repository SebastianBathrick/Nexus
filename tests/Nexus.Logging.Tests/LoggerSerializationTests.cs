using System.Text.Json;
namespace Nexus.Logging.Tests;

[TestFixture]
public class LoggerSerializationTests
{
    // Serialize — property values

    [Test]
    public void Serialize_DefaultState_ReturnsValidJson()
    {
        var logger = new CapturingLogger();

        var json = logger.Serialize();
        using var doc = JsonDocument.Parse(json);

        Assert.That(doc.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Object));
    }

    [Test]
    public void Serialize_MinimumLogLevel_IsEnumName()
    {
        var logger = new CapturingLogger { MinimumLogLevel = LogLevel.Warning };

        var json = logger.Serialize();
        using var doc = JsonDocument.Parse(json);
        var levelElement = doc.RootElement.GetProperty("MinimumLogLevel");

        Assert.That(levelElement.ValueKind, Is.EqualTo(JsonValueKind.String));
        Assert.That(levelElement.GetString(), Is.EqualTo("Warning"));
    }

    [Test]
    public void Serialize_AllProperties_ArePresentInOutput()
    {
        var logger = new CapturingLogger();

        var json = logger.Serialize();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.That(root.TryGetProperty("IsEnabled", out _), Is.True);
        Assert.That(root.TryGetProperty("MinimumLogLevel", out _), Is.True);
        Assert.That(root.TryGetProperty("IsLabelsEnabled", out _), Is.True);
        Assert.That(root.TryGetProperty("IsSeparatorEnabled", out _), Is.True);
        Assert.That(root.TryGetProperty("LabelFormat", out _), Is.True);
    }

    [Test]
    public void Serialize_ReflectsCurrentValues()
    {
        var logger = new CapturingLogger
        {
            MinimumLogLevel = LogLevel.None,
            IsLabelsEnabled = false,
            IsSeparatorEnabled = true,
            LabelFormat = "[{Level}]:"
        };

        var json = logger.Serialize();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.That(root.GetProperty("IsEnabled").GetBoolean(), Is.False);
        Assert.That(root.GetProperty("MinimumLogLevel").GetString(), Is.EqualTo("None"));
        Assert.That(root.GetProperty("IsLabelsEnabled").GetBoolean(), Is.False);
        Assert.That(root.GetProperty("IsSeparatorEnabled").GetBoolean(), Is.True);
        Assert.That(root.GetProperty("LabelFormat").GetString(), Is.EqualTo("[{Level}]:"));
    }

    [Test]
    public void Serialize_NullLabelFormat_SerializesAsNull()
    {
        var logger = new CapturingLogger { LabelFormat = null };

        var json = logger.Serialize();
        using var doc = JsonDocument.Parse(json);

        Assert.That(doc.RootElement.GetProperty("LabelFormat").ValueKind, Is.EqualTo(JsonValueKind.Null));
    }

    // Deserialize — sets properties

    [Test]
    public void Deserialize_FullJson_SetsAllProperties()
    {
        var logger = new CapturingLogger();
        const string json = """{"IsEnabled":false,"IsLabelsEnabled":false,"IsSeparatorEnabled":true,"LabelFormat":"[test]:"}""";

        logger.Deserialize(json);

        Assert.That(logger.IsEnabled(), Is.False);
        Assert.That(logger.MinimumLogLevel, Is.EqualTo(LogLevel.None));
        Assert.That(logger.IsLabelsEnabled, Is.False);
        Assert.That(logger.IsSeparatorEnabled, Is.True);
        Assert.That(logger.LabelFormat, Is.EqualTo("[test]:"));
    }

    [Test]
    public void Deserialize_MinimumLogLevel_ParsedFromString()
    {
        var logger = new CapturingLogger();

        logger.Deserialize("""{"MinimumLogLevel":"Error"}""");

        Assert.That(logger.MinimumLogLevel, Is.EqualTo(LogLevel.Error));
    }

    [TestCase("Verbose", LogLevel.Verbose)]
    [TestCase("Debug", LogLevel.Debug)]
    [TestCase("Info", LogLevel.Info)]
    [TestCase("Warning", LogLevel.Warning)]
    [TestCase("Error", LogLevel.Error)]
    [TestCase("Critical", LogLevel.Critical)]
    public void Deserialize_AllLogLevels_ParseCorrectly(string levelName, LogLevel expected)
    {
        var logger = new CapturingLogger();

        logger.Deserialize($$"""{"MinimumLogLevel":"{{levelName}}"}""");

        Assert.That(logger.MinimumLogLevel, Is.EqualTo(expected));
    }

    [Test]
    public void Deserialize_PartialJson_OnlyUpdatesProvidedFields()
    {
        var logger = new CapturingLogger
        {
            MinimumLogLevel = LogLevel.Warning,
            IsLabelsEnabled = false,
            IsSeparatorEnabled = true,
            LabelFormat = "custom"
        };

        logger.Deserialize("""{"IsEnabled":false}""");

        Assert.That(logger.IsEnabled(), Is.False);
        Assert.That(logger.MinimumLogLevel, Is.EqualTo(LogLevel.None));
        Assert.That(logger.IsLabelsEnabled, Is.False);
        Assert.That(logger.IsSeparatorEnabled, Is.True);
        Assert.That(logger.LabelFormat, Is.EqualTo("custom"));
    }

    [Test]
    public void Deserialize_EmptyJson_LeavesAllPropertiesUnchanged()
    {
        var logger = new CapturingLogger
        {
            MinimumLogLevel = LogLevel.Debug,
            IsLabelsEnabled = false,
            IsSeparatorEnabled = true,
            LabelFormat = "test"
        };

        logger.Deserialize("{}");

        Assert.That(logger.IsEnabled(), Is.True);
        Assert.That(logger.MinimumLogLevel, Is.EqualTo(LogLevel.Debug));
        Assert.That(logger.IsLabelsEnabled, Is.False);
        Assert.That(logger.IsSeparatorEnabled, Is.True);
        Assert.That(logger.LabelFormat, Is.EqualTo("test"));
    }

    [Test]
    public void Deserialize_UnknownProperties_AreIgnored()
    {
        var logger = new CapturingLogger();

        logger.Deserialize("""{"UnknownKey":"value","AnotherKey":42}""");

        Assert.That(logger.IsEnabled(), Is.True);
    }

    [Test]
    public void Deserialize_NullLabelFormat_SetsLabelFormatToNull()
    {
        var logger = new CapturingLogger { LabelFormat = "something" };

        logger.Deserialize("""{"LabelFormat":null}""");

        Assert.That(logger.LabelFormat, Is.Null);
    }

    // Round-trip

    [Test]
    public void SerializeDeserialize_RoundTrip_ProducesIdenticalConfiguration()
    {
        var original = new CapturingLogger
        {
            MinimumLogLevel = LogLevel.None,
            IsLabelsEnabled = false,
            IsSeparatorEnabled = true,
            LabelFormat = "[{Level}]:"
        };

        var json = original.Serialize();

        var restored = new CapturingLogger();
        restored.Deserialize(json);

        Assert.That(restored.IsEnabled(), Is.EqualTo(original.IsEnabled()));
        Assert.That(restored.MinimumLogLevel, Is.EqualTo(original.MinimumLogLevel));
        Assert.That(restored.IsLabelsEnabled, Is.EqualTo(original.IsLabelsEnabled));
        Assert.That(restored.IsSeparatorEnabled, Is.EqualTo(original.IsSeparatorEnabled));
        Assert.That(restored.LabelFormat, Is.EqualTo(original.LabelFormat));
    }
}
