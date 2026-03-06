using System;

namespace Nexus.Logging
{
    public static class Log
    {
        static ILogger? _logger = new DummyLogger();

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static void Info(string msg, params object[]? props) => _logger?.Info(msg, props);

        public static void Debug(string msg, params object[]? props) => _logger?.Debug(msg, props);

        public static void Warning(string msg, params object[]? props) => _logger?.Warning(msg, props);

        public static void Error(string msg, params object[]? props) => _logger?.Error(msg, props);

        public static void Error(Exception ex, string msg, params object[]? props) => _logger?.Error(ex, msg, props);

        public static void Critical(string msg, params object[]? props) => _logger?.Critical(msg, props);

        public static void Critical(Exception ex, string msg, params object[]? props) => _logger?.Critical(ex, msg, props);
    }
}
