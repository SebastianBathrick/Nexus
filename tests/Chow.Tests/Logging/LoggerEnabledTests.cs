using Chow.Logging;

namespace Chow.Tests;

[TestFixture]
public class LoggerEnabledTests
{
    [Test]
    public void IsEnabled_False_AllMethods_ProduceNoOutput()
    {
        var logger = new CapturingLogger(false);
        var ex = new Exception("test");

        logger.Verbose("msg");
        logger.Debug("msg");
        logger.Info("msg");
        logger.Warning("msg");
        logger.Error("msg");
        logger.Error(ex, "msg");
        logger.Critical("msg");
        logger.Critical(ex, "msg");

        Assert.That(logger.CapturedMessages, Is.Empty);
    }

    [Test]
    public void IsEnabled_True_Info_ProducesOutput()
    {
        var logger = new CapturingLogger(true);

        logger.Info("hello");

        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
    }

    [Test]
    public void IsLabelsEnabled_False_OutputIsRawMessage()
    {
        var logger = new CapturingLogger(true) { IsLabelsEnabled = false };

        logger.Info("hello world");

        Assert.That(logger.CapturedMessages[0], Is.EqualTo("hello world"));
    }

    [Test]
    public void IsLabelsEnabled_True_OutputContainsLabel()
    {
        var logger = new CapturingLogger(true) { IsLabelsEnabled = true };

        logger.Info("hello");

        Assert.That(logger.CapturedMessages[0], Does.Contain("["));
    }

    [Test]
    public void SetFormat_ChangesLabelPrefix()
    {
        var logger = new CapturingLogger(true);
        logger.SetFormat("[{Level}]:");

        logger.Info("hello");

        Assert.That(logger.CapturedMessages[0], Does.StartWith("[Info]:"));
    }

    [Test]
    public void SetFormat_EmptyString_OutputIsRawMessage()
    {
        var logger = new CapturingLogger(true);
        logger.SetFormat("");

        logger.Info("hello");

        Assert.That(logger.CapturedMessages[0], Is.EqualTo("hello"));
    }
}
