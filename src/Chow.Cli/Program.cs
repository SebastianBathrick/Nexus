using Chow.Logging;

namespace Chow.Cli;

public static class Program
{
    const char ArgJoinSeparator = ' ';
    const string InfoFlag = "--info";
    const string DebugFlag = "--debug";
    const string HelpFlag = "--help";
    const string RequiredFileExtension = ".chw";
    const string ImplicitEntryPointScript = "Main" + RequiredFileExtension;

    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0 || args[0] == HelpFlag)
        {
            var implicitPath = Path.Combine(Directory.GetCurrentDirectory(), ImplicitEntryPointScript);
            if (args.Length == 0 && File.Exists(implicitPath))
            {
                var implicitCode = await ReadFileAsync(implicitPath);
                if (implicitCode == null) return 1;
                Console.WriteLine(ChowEngine.Run(implicitCode).ToString());
                return 0;
            }

            PrintHelp();
            return 0;
        }

        var logLevel = ParseFlag(args, out var codeArgs);
        if (logLevel == LogLevel.None) return 1;

        var srcCode = await ResolveSourceAsync(codeArgs);
        if (srcCode == null) return 1;

        var result = logLevel.HasValue
            ? ChowEngine.Run<ConsoleLogger>(srcCode, logLevel.Value)
            : ChowEngine.Run(srcCode);

        Console.WriteLine(result.ToString());
        return 0; // TODO: Return the source code's exit code once better error handling is added
    }

    // Returns null if no flag, the parsed LogLevel if a valid flag was found, or LogLevel.None on error.
    static LogLevel? ParseFlag(string[] args, out string[] codeArgs)
    {
        codeArgs = args;

        if (args[0][0] != '-')
            return null;

        if (args[0] == DebugFlag)
        {
            codeArgs = args.Skip(1).ToArray();
            return LogLevel.Debug;
        }

        if (args[0] == InfoFlag)
        {
            codeArgs = args.Skip(1).ToArray();
            return LogLevel.Info;
        }

        Console.WriteLine($"Error: Unknown flag '{args[0]}'");
        return LogLevel.None;
    }

    static void PrintHelp()
    {
        Console.WriteLine("Chow - Scripting language");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine($"  chow                        Run {ImplicitEntryPointScript} in the current directory");
        Console.WriteLine("  chow <file.chw>             Run a script file");
        Console.WriteLine("  chow <source code>          Run inline source code");
        Console.WriteLine("  chow --help                 Show this help text");
        Console.WriteLine();
        Console.WriteLine("Flags (optional, must come first):");
        Console.WriteLine($"  {InfoFlag}                     Enable info-level logging");
        Console.WriteLine($"  {DebugFlag}                    Enable debug-level logging");
    }

    static async Task<string?> ResolveSourceAsync(string[] codeArgs)
    {
        if (codeArgs.Length == 0)
            return await ResolveImplicitEntryPointAsync();

        if (codeArgs[0].EndsWith(RequiredFileExtension))
            return await ReadFileAsync(codeArgs[0]);

        return string.Join(ArgJoinSeparator, codeArgs);
    }

    static async Task<string?> ResolveImplicitEntryPointAsync()
    {
        var implicitPath = Path.Combine(Directory.GetCurrentDirectory(), ImplicitEntryPointScript);

        if (File.Exists(implicitPath))
            return await ReadFileAsync(implicitPath);
        
        Console.WriteLine($"Error: No script provided and '{ImplicitEntryPointScript}' was not found in the current directory. " +
                              $"Pass a script path or place a '{ImplicitEntryPointScript}' file here.");
            return null;
    }

    static async Task<string?> ReadFileAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Error: File path cannot be empty");
            return null;
        }

        if (!File.Exists(path))
        {
            Console.WriteLine($"Error: File not found '{path}'");
            return null;
        }

        try
        {
            return await File.ReadAllTextAsync(path);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Error: Access denied to file '{path}'");
            return null;
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error: Failed to read file '{path}': {e.Message}");
            return null;
        }
    }
}