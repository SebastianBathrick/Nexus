namespace Chow.Logging.Tests;

class CapturingLogger : Logger
{
    public CapturingLogger(bool isEnabled = true, LogLevel minLogLvl = LogLevel.Verbose)
        : base(isEnabled ? minLogLvl : LogLevel.None)
    {
    }

    public List<string> CapturedMessages { get; } = new();

    protected override void OutputFormattedMessage(string msg)
        => CapturedMessages.Add(msg);
}
