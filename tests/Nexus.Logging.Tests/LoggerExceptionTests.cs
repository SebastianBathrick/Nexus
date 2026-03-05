namespace Nexus.Logging.Tests;

[TestFixture]
public class LoggerExceptionTests
{
    static Exception CreateException()
    {
        try
        {
            throw new InvalidOperationException("test error");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    [Test]
    public void Error_WithException_OutputContainsExceptionMessage()
    {
        var logger = new CapturingLogger { IsLabelsEnabled = false };
        var ex = CreateException();

        logger.Error(ex, "Outer");

        Assert.That(logger.CapturedMessages[0], Does.Contain("test error"));
    }

    [Test]
    public void Error_WithException_OutputContainsStackTrace()
    {
        var logger = new CapturingLogger { IsLabelsEnabled = false };
        var ex = CreateException();

        logger.Error(ex, "Outer");

        Assert.That(logger.CapturedMessages[0], Does.Contain("CreateException"));
    }

    [Test]
    public void Critical_WithException_OutputContainsExceptionMessage()
    {
        var logger = new CapturingLogger { IsLabelsEnabled = false };
        var ex = CreateException();

        logger.Critical(ex, "Outer");

        Assert.That(logger.CapturedMessages[0], Does.Contain("test error"));
    }

    [Test]
    public void Error_WithExceptionAndProps_SubstitutesProps()
    {
        var logger = new CapturingLogger { IsLabelsEnabled = false };
        var ex = CreateException();

        logger.Error(ex, "Failed in {op}", "DatabaseConnect");

        Assert.That(logger.CapturedMessages[0], Does.StartWith("Failed in DatabaseConnect"));
    }

    [Test]
    public void Error_WithException_IsFilteredByMinLogLevel()
    {
        var logger = new CapturingLogger(minLogLvl: LogLevel.Critical);
        var ex = CreateException();

        logger.Error(ex, "msg");

        Assert.That(logger.CapturedMessages, Is.Empty);
    }
}
