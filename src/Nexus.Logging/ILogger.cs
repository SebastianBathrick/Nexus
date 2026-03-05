using System;

namespace Nexus.Logging
{
    public interface ILogger
    {
        public LogLevel MinimumLogLevel { get; set; }
        
        public bool IsLabelsEnabled { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSeparatorEnabled { get; set; }
        
        public void SetFormat(string format);

        string Serialize();
        void Deserialize(string json);

        void Verbose(string msg, params object[]? props);
        void Debug(string msg, params object[]? props);
        void Info(string msg, params object[]? props);
        void Warning(string msg, params object[]? props);

        void Error(string msg, params object[]? props);
        void Error(Exception ex, string msg, params object[]? props);
        
        void Critical(string msg, params object[]? props);
        void Critical(Exception ex, string msg, params object[]? props);
    }
}