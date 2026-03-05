namespace Nexus.Logging.Tests;

[TestFixture]
public class LogFacadeTests
{
    [TearDown]
    public void TearDown() => Log.SetLogger(new CapturingLogger(isEnabled: false));

    [Test]
    public void Default_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => Log.Info("msg"));
    }

    [Test]
    public void SetLogger_Info_DelegatesToLogger()
    {
        var logger = new CapturingLogger() { IsLabelsEnabled = false };
        Log.SetLogger(logger);

        Log.Info("hello");

        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(1));
        Assert.That(logger.CapturedMessages[0], Is.EqualTo("hello"));
    }

    [Test]
    public void SetLogger_AllMethods_Delegate()
    {
        var logger = new CapturingLogger() { IsLabelsEnabled = false };
        Log.SetLogger(logger);

        var ex = new Exception("err");
        Log.Debug("a");
        Log.Info("b");
        Log.Warning("c");
        Log.Error("d");
        Log.Error(ex, "e");
        Log.Critical("f");
        Log.Critical(ex, "g");

        Assert.That(logger.CapturedMessages, Has.Count.EqualTo(7));
    }

    [Test]
    public void SetLogger_Replace_UsesNewLogger()
    {
        var first = new CapturingLogger() { IsLabelsEnabled = false };
        var second = new CapturingLogger() { IsLabelsEnabled = false };

        Log.SetLogger(first);
        Log.SetLogger(second);
        Log.Info("msg");

        Assert.That(first.CapturedMessages, Is.Empty);
        Assert.That(second.CapturedMessages, Has.Count.EqualTo(1));
    }
}
