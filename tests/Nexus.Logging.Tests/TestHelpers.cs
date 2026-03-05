namespace Nexus.Logging.Tests;

internal class CapturingLogger : Logger
{
    public List<string> CapturedMessages { get; } = new();

    public CapturingLogger(bool isEnabled = true, LogLevel minLogLvl = LogLevel.Verbose)
        : base(isEnabled ? minLogLvl : LogLevel.None) { }

    protected override void OutputFormattedMessage(string msg)
        => CapturedMessages.Add(msg);
}
