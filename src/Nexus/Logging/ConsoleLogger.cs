using System;

namespace Nexus.Logging
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(LogLevel minLogLvl = LogLevel.Info)
            : base(minLogLvl)
        {
        }

        protected override void OutputFormattedMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}