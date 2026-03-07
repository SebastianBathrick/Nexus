using Nexus.Compilation;
using Nexus.Execution;
using Nexus.Execution.Values;
using Nexus.Lexing;
using Nexus.Logging;
using Nexus.Parsing;
using System.Diagnostics;

namespace Nexus
{
    public static class NexusInterpreter
    {
        const string TimeFormat = @"fff\.ffffff";
        const LogLevel DefaultMinimumLogLevel = LogLevel.Info;

        public static NexusValue Run<TLoggerType>(string sourceCode, LogLevel minLogLevel = DefaultMinimumLogLevel) where TLoggerType : ILogger, new() => 
            Run(sourceCode, new TLoggerType() { MinimumLogLevel = minLogLevel });
        
        public static NexusValue Run(string sourceCode, ILogger? logger = null)
        {
            // Measures individual execution times for each step (lexing, syntax analysis, compilation, execution)
            Stopwatch? stepTimer = null;

            // Measures total execution time for the entire pipeline 
            Stopwatch? totTimer = null;

            if (logger is not null && logger.MinimumLogLevel > LogLevel.None)
            {
                stepTimer = new();
                totTimer = new();
            }
            
            logger?.Debug("\n\nSource Code:\n\t{Code}\n", sourceCode);

            // [Lexical analysis]=======================================================================================
            totTimer?.Start();
            stepTimer?.Start();
            
            var tokenCollection = Lexer.Lex(sourceCode);
            
            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("LEXING DURATION: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Debug("\n\nTokens:\n\t{Tokens}\n", tokenCollection);

            // [Syntax analysis]========================================================================================
            stepTimer?.Reset();
            totTimer?.Start();
            stepTimer?.Start();
            
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("PARSING DURATION: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Debug("\n\nSyntax tree:\n{SyntaxTree}", syntaxTreeRoot);

            // [Compilation]============================================================================================
            stepTimer?.Reset();
            totTimer?.Start();
            stepTimer?.Start();

            var topLvlChunk = ChunkCompiler.CompileTopLevel(syntaxTreeRoot);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("COMPILATION DURATION: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Debug("\n\nBytecode:\n{Bytecode}", topLvlChunk);

            // [Execution]==============================================================================================
            stepTimer?.Reset();
            totTimer?.Start();
            stepTimer?.Start();

            var topLvlReturnVal = VirtualMachine.ExecuteTopLevel(topLvlChunk);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("VM EXECUTION DURATION: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Info("TOTAL INTERPRETER DURATION: {Time}ms", totTimer?.Elapsed.ToString(TimeFormat));
            logger?.Info("NOTE: Measurements include time tracking overhead. Individual step times are more accurate than the total time.");
            
            return topLvlReturnVal;
        }
    }
}
