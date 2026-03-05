namespace Nexus.Logging
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(bool isEnabled = false, LogLevel minLogLvl = LogLevel.Info)
            : base(isEnabled, minLogLvl)
        {
        }

        protected override void OutputFormattedMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
