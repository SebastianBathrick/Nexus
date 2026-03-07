namespace Chow.Logging.Tests;

[TestFixture]
public class LoggerFormatLabelTests
{
    [Test]
    public void FormatLabel_DefaultFormat_IncludesLevelAndTimestamp()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2024, 12, 25, 15, 30, 45);

        var result = logger.FormatLabelPublic("[{LogLevel} | {Timestamp:MM-dd HH:mm:ss}]:", LogLevel.Info, timestamp);

        Assert.That(result, Is.EqualTo("[Info | 12-25 15:30:45]:"));
    }

    [Test]
    public void FormatLabel_LogLevel_SubstitutesAllLevels()
    {
        var logger = new TestLogger();
        var timestamp = DateTime.Now;

        Assert.That(logger.FormatLabelPublic("[{LogLevel}]", LogLevel.Verbose, timestamp), Is.EqualTo("[Verbose]"));
        Assert.That(logger.FormatLabelPublic("[{LogLevel}]", LogLevel.Debug, timestamp), Is.EqualTo("[Debug]"));
        Assert.That(logger.FormatLabelPublic("[{LogLevel}]", LogLevel.Info, timestamp), Is.EqualTo("[Info]"));
        Assert.That(logger.FormatLabelPublic("[{LogLevel}]", LogLevel.Warning, timestamp), Is.EqualTo("[Warning]"));
        Assert.That(logger.FormatLabelPublic("[{LogLevel}]", LogLevel.Error, timestamp), Is.EqualTo("[Error]"));
        Assert.That(logger.FormatLabelPublic("[{LogLevel}]", LogLevel.Critical, timestamp), Is.EqualTo("[Critical]"));
    }

    [Test]
    public void FormatLabel_Level_SubstitutesAllLevels()
    {
        var logger = new TestLogger();
        var timestamp = DateTime.Now;

        Assert.That(logger.FormatLabelPublic("[{Level}]", LogLevel.Verbose, timestamp), Is.EqualTo("[Verbose]"));
        Assert.That(logger.FormatLabelPublic("[{Level}]", LogLevel.Debug, timestamp), Is.EqualTo("[Debug]"));
        Assert.That(logger.FormatLabelPublic("[{Level}]", LogLevel.Info, timestamp), Is.EqualTo("[Info]"));
        Assert.That(logger.FormatLabelPublic("[{Level}]", LogLevel.Warning, timestamp), Is.EqualTo("[Warning]"));
        Assert.That(logger.FormatLabelPublic("[{Level}]", LogLevel.Error, timestamp), Is.EqualTo("[Error]"));
        Assert.That(logger.FormatLabelPublic("[{Level}]", LogLevel.Critical, timestamp), Is.EqualTo("[Critical]"));
    }

    [Test]
    public void FormatLabel_TimestampWithFormat_AppliesFormat()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2024, 3, 4, 9, 5, 3);

        Assert.That(
            logger.FormatLabelPublic("{Timestamp:yyyy-MM-dd}", LogLevel.Info, timestamp),
            Is.EqualTo("2024-03-04"));

        Assert.That(
            logger.FormatLabelPublic("{Timestamp:HH:mm:ss}", LogLevel.Info, timestamp),
            Is.EqualTo("09:05:03"));

        Assert.That(
            logger.FormatLabelPublic("{Timestamp:MMM dd, yyyy}", LogLevel.Info, timestamp),
            Is.EqualTo("Mar 04, 2024"));
    }

    [Test]
    public void FormatLabel_TimestampWithoutFormat_UsesDefaultToString()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2024, 3, 4, 15, 30, 45);

        var result = logger.FormatLabelPublic("{Timestamp}", LogLevel.Info, timestamp);

        Assert.That(result, Is.EqualTo("3/4/2024 3:30:45 PM"));
    }

    [Test]
    public void FormatLabel_VariousFormats_SubstitutesLogLevel()
    {
        var logger = new TestLogger();

        Assert.That(logger.FormatLabelPublic("[{LogLevel}]:", LogLevel.Warning, DateTime.Now), Is.EqualTo("[Warning]:"));
        Assert.That(logger.FormatLabelPublic("{Level} - ", LogLevel.Error, DateTime.Now), Is.EqualTo("Error - "));
        Assert.That(logger.FormatLabelPublic(">> {LogLevel} >> ", LogLevel.Debug, DateTime.Now), Is.EqualTo(">> Debug >> "));
    }

    [Test]
    public void FormatLabel_CombinedLevelAndTimestamp_SubstitutesBoth()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2026, 3, 4, 10, 20, 30);

        var result = logger.FormatLabelPublic(
            "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{LogLevel}]:",
            LogLevel.Error,
            timestamp);

        Assert.That(result, Is.EqualTo("[2026-03-04 10:20:30] [Error]:"));
    }

    [Test]
    public void FormatLabel_NoPlaceholders_ReturnsLiteral()
    {
        var logger = new TestLogger();
        var result = logger.FormatLabelPublic("LOG: ", LogLevel.Info, DateTime.Now);
        Assert.That(result, Is.EqualTo("LOG: "));
    }

    [Test]
    public void FormatLabel_UnknownPlaceholder_LeavesAsIs()
    {
        var logger = new TestLogger();
        var result = logger.FormatLabelPublic("[{Unknown}]", LogLevel.Info, DateTime.Now);
        Assert.That(result, Is.EqualTo("[{Unknown}]"));
    }

    [Test]
    public void FormatLabel_InvalidTimestampFormat_ReturnsFormatString()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2024, 3, 4, 15, 30, 45);

        var result = logger.FormatLabelPublic("{Timestamp:INVALID}", LogLevel.Info, timestamp);

        Assert.That(result, Is.EqualTo("INVALID"));
    }

    [Test]
    public void FormatLabel_UnclosedBrace_LeavesLiteral()
    {
        var logger = new TestLogger();
        var result = logger.FormatLabelPublic("[{LogLevel", LogLevel.Info, DateTime.Now);
        Assert.That(result, Is.EqualTo("[{LogLevel"));
    }

    [Test]
    public void FormatLabel_EmptyString_ReturnsEmptyString()
    {
        var logger = new TestLogger();
        var result = logger.FormatLabelPublic("", LogLevel.Info, DateTime.Now);
        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public void FormatLabel_MultipleTimestampsWithDifferentFormats_AppliesEachFormat()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2024, 12, 25, 15, 30, 45);

        var result = logger.FormatLabelPublic(
            "{Timestamp:yyyy-MM-dd} at {Timestamp:HH:mm:ss}",
            LogLevel.Info,
            timestamp);

        Assert.That(result, Is.EqualTo("2024-12-25 at 15:30:45"));
    }

    [Test]
    public void FormatLabel_ComplexFormat_SubstitutesAll()
    {
        var logger = new TestLogger();
        var timestamp = new DateTime(2024, 3, 4, 9, 5, 3);

        var result = logger.FormatLabelPublic(
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}]:",
            LogLevel.Critical,
            timestamp);

        Assert.That(result, Is.EqualTo("[2024-03-04 09:05:03.000] [Critical]:"));
    }
}

class TestLogger : Logger
{
    public string FormatLabelPublic(string labelFormat, LogLevel level, DateTime timestamp)
        => FormatLabel(labelFormat, level, timestamp);

    protected override void OutputFormattedMessage(string msg)
    {
    }
}
