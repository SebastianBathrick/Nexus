using Nexus.Logging;
using System;

namespace Nexus.Logging
{
    // Does nothing when methods are called
    internal class DummyLogger : ILogger
    {
        public bool IsLabelsEnabled { get; set; }
        public bool IsEnabled() => false;
        public void SetIsEnabled(bool value) { }
        public bool IsSeparatorEnabled { get; set; }
        public LogLevel MinimumLogLevel { get; set; }
        public string? LabelFormat { get; set; }

        public void SetFormat(string format)
        {
        }

        public string Serialize() => "{}";
        public void Deserialize(string json) { }

        public void Verbose(string msg, params object[]? props) {}
        public void Debug(string msg, params object[]? props) { }
        public void Info(string msg, params object[]? props) { }
        public void Warning(string msg, params object[]? props) { }
        public void Error(string msg, params object[]? props) { }
        public void Error(Exception ex, string msg, params object[]? props) { }
        public void Critical(string msg, params object[]? props) { }
        public void Critical(Exception ex, string msg, params object[]? props) { }
    }
}