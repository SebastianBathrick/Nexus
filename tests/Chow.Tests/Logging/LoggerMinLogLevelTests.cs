using Chow.Logging;

namespace Chow.Tests;

[TestFixture]
public class LoggerMinLogLevelTests
{
    [Test]
    public void Verbose_AtVerboseMinLevel_Passes()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Verbose);
        logger.Verbose("msg");
        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void Verbose_BelowDebugMinLevel_Filtered()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Debug);
        logger.Verbose("msg");
        Assert.That(logger.CapturedMessages, Is.Empty);
    }

    [Test]
    public void Debug_AtDebugMinLevel_Passes()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Debug);
        logger.Debug("msg");
        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void Debug_BelowInfoMinLevel_Filtered()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Info);
        logger.Debug("msg");
        Assert.That(logger.CapturedMessages, Is.Empty);
    }

    [Test]
    public void Info_AtInfoMinLevel_Passes()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Info);
        logger.Info("msg");
        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void Info_BelowWarningMinLevel_Filtered()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Warning);
        logger.Info("msg");
        Assert.That(logger.CapturedMessages, Is.Empty);
    }

    [Test]
    public void Warning_AtWarningMinLevel_Passes()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Warning);
        logger.Warning("msg");
        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void Error_AtErrorMinLevel_Passes()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Error);
        logger.Error("msg");
        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void Critical_AtCriticalMinLevel_Passes()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Critical);
        logger.Critical("msg");
        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void MinLogLevel_Verbose_AllMethodsProduce()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Verbose);

        logger.Verbose("msg");
        logger.Debug("msg");
        logger.Info("msg");
        logger.Warning("msg");
        logger.Error("msg");
        logger.Critical("msg");

        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(6));
    }

    [Test]
    public void MinLogLevel_Critical_OnlyCriticalProduces()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Critical);

        logger.Verbose("msg");
        logger.Debug("msg");
        logger.Info("msg");
        logger.Warning("msg");
        logger.Error("msg");
        logger.Critical("msg");

        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }
}
