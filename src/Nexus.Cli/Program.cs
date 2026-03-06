using Nexus.Execution.Values;

namespace Nexus.Cli;

public static class Program
{
    const char ArgumentJoinSeperator = ' ';

    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Error: No arguments provided");
            return 1;
        }
        
        var srcCode = String.Join(ArgumentJoinSeperator, args);
        var result = NexusInterpreter.Run(srcCode);
        
        Console.WriteLine(result.ToString());
        return result is NexusNumber ? result.ToInt() : 0;
    }
}