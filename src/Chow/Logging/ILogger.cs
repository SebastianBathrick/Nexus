using System;
namespace Chow.Logging
{
    public interface ILogger
    {
        LogLevel MinimumLogLevel { get; set; }

        bool IsLabelsEnabled { get; set; }
        bool IsSeparatorEnabled { get; set; }

        bool IsEnabled();

        void SetIsEnabled(bool value);

        void SetFormat(string format);

        void Verbose(string msg, params object[] props);

        void Debug(string msg, params object[] props);

        void Info(string msg, params object[] props);

        void Warning(string msg, params object[] props);

        void Error(string msg, params object[] props);

        void Error(Exception ex, string msg, params object[] props);

        void Critical(string msg, params object[] props);

        void Critical(Exception ex, string msg, params object[] props);
    }
}
