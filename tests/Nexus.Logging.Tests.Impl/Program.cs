using Nexus.Logging;

var logger = new ConsoleLogger(minLogLvl: LogLevel.Verbose)
{
    IsSeparatorEnabled = true
};

// Basic usage
Console.WriteLine("=== Basic Logging ===");
logger.Info("Application started");
logger.Warning("Low disk space: {percent}% remaining", 15);
logger.Error("Failed to connect to {host}", "db.example.com");

// MinimumLogLevel filtering
Console.WriteLine("\n=== MinimumLogLevel Filtering (Warning+) ===");
logger.MinimumLogLevel = LogLevel.Warning;
logger.Debug("This will not appear");
logger.Info("This will not appear");
logger.Warning("This appears");
logger.MinimumLogLevel = LogLevel.Verbose;

// Property substitution
Console.WriteLine("\n=== Property Substitution ===");
logger.Info("User {UserId} logged in from {IP}", 42, "10.0.0.1");
logger.Info("{method} completed in {ms}ms", "ProcessOrder", 142);
logger.Warning("Retry {attempt} of {max} for {op}", 2, 3, "ExportData");

// Custom label format
Console.WriteLine("\n=== Custom Label Format ===");
logger.SetFormat("[{Level} | {Timestamp:HH:mm:ss}]:");
logger.Info("Using custom format");
logger.Warning("Still custom format");

// Labels toggle
Console.WriteLine("\n=== Labels Toggle ===");
logger.IsLabelsEnabled = false;
logger.Info("No label on this line");
logger.IsLabelsEnabled = true;
logger.SetFormat("[{LogLevel} | {Timestamp:MM-dd HH:mm:ss}]:");
logger.Info("Label is back");

// Exception logging
Console.WriteLine("\n=== Exception Logging ===");
try
{
    throw new InvalidOperationException("Connection timeout after 30s");
}
catch (Exception ex)
{
    logger.Error(ex, "Failed in {op}", "DatabaseConnect");
}

// Log static facade
Console.WriteLine("\n=== Log Static Facade ===");
Log.SetLogger(logger);
Log.Info("Using the static facade");
Log.Warning("Facade warning: {reason}", "high memory usage");
