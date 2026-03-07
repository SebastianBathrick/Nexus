using System;
namespace Chow.Logging
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger()
        {
        }

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
