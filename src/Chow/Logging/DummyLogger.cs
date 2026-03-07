using System;
namespace Chow.Logging
{
// Does nothing when methods are called
    class DummyLogger : ILogger
    {
        public string? LabelFormat { get; set; }
        public bool IsLabelsEnabled { get; set; }

        public bool IsEnabled() => false;

        public void SetIsEnabled(bool value)
        {
        }

        public bool IsSeparatorEnabled { get; set; }
        public LogLevel MinimumLogLevel { get; set; }

        public void SetFormat(string format)
        {
        }

        public void Verbose(string msg, params object?[] props)
        {
        }

        public void Debug(string msg, params object?[] props)
        {
        }

        public void Info(string msg, params object?[] props)
        {
        }

        public void Warning(string msg, params object?[] props)
        {
        }

        public void Error(string msg, params object?[] props)
        {
        }

        public void Error(Exception ex, string msg, params object?[] props)
        {
        }

        public void Critical(string msg, params object?[] props)
        {
        }

        public void Critical(Exception ex, string msg, params object?[] props)
        {
        }
    }
}
