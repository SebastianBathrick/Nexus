using Nexus;
using Nexus.Execution.Values;
using Nexus.Logging;

public static class Program
{
    const char ArgumentJoinSeperator = ' ';
    const char FlagPrefixChar = '-';
    const string DeveloperInfoFlag =  "--info";
    const string DeveloperDebugFlag = "--debug";
    const int FlagArgumentIndex = 0;
    const int FlagCodeStartIndexOffset = 1;
    
    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Error: No arguments provided");
            return 1;
        }

        NexusValue result;

        // Checks to see if the argument's first character is '-' (no statement in the source code can start with a '-')
        if (args[FlagArgumentIndex].Length > 0 && args[FlagArgumentIndex][0] == FlagPrefixChar)
        {
            LogLevel minLogLevel;

            if (args[FlagArgumentIndex] == DeveloperDebugFlag)
                minLogLevel = LogLevel.Debug;
            else if (args[FlagArgumentIndex] == DeveloperInfoFlag)
                minLogLevel = LogLevel.Info;
            else
            {
                Console.WriteLine("Error: Expected flag starting with {0} but got {1}", FlagPrefixChar, args[FlagArgumentIndex]);
                return 1;
            }
            
            var srcCode = string.Join(ArgumentJoinSeperator, args.Skip(FlagCodeStartIndexOffset));
            result = NexusInterpreter.Run<ConsoleLogger>(srcCode, minLogLevel);
        }
        else
        {
            var srcCode = string.Join(ArgumentJoinSeperator, args);
            result = NexusInterpreter.Run(srcCode);
        }
        

        
        Console.WriteLine(result.ToString());
        return result is NexusNumber ? result.ToInt() : 0;
    }
}