using System.Diagnostics;
using Chow.Compilation;
using Chow.Interpretation;
using Chow.Lexing;
using Chow.Logging;
using Chow.Parsing;
namespace Chow
{
    public static class ChowEngine
    {
        const string TimeFormat = @"fff\.ffffff";
        const LogLevel DefaultMinimumLogLevel = LogLevel.Info;

        public static TaggedUnion Run<TLoggerType>(string sourceCode, LogLevel minLogLevel = DefaultMinimumLogLevel) where TLoggerType : ILogger, new() =>
            Run(sourceCode, new TLoggerType { MinimumLogLevel = minLogLevel });

        public static TaggedUnion Run(string sourceCode, ILogger logger = null)
        {
            // Measures individual execution times for each step (lexing, syntax analysis, compilation, execution)
            Stopwatch stepTimer = null;

            // Measures total execution time for the entire pipeline
            Stopwatch totTimer = null;

            if (logger != null && logger.MinimumLogLevel > LogLevel.None)
            {
                stepTimer = new Stopwatch();
                totTimer = new Stopwatch();
            }

            logger?.Debug("\n\nSource Code:\n\t{Code}\n", sourceCode);

            // [Lexical analysis]=======================================================================================
            totTimer?.Start();
            stepTimer?.Start();

            var tokenCollection = Lexer.Lex(sourceCode);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("LEX EXECUTION TIME: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Debug("\n\nTokens:\n{Tokens}\n", tokenCollection);

            // [Syntax analysis]========================================================================================
            stepTimer?.Reset();
            totTimer?.Start();
            stepTimer?.Start();

            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("PARSE EXECUTION TIME: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Debug("\n\nSyntax tree:\n{SyntaxTree}", syntaxTreeRoot);

            // [Compilation]============================================================================================
            stepTimer?.Reset();
            totTimer?.Start();
            stepTimer?.Start();

            var topLvlChunk = ChunkCompiler.CompileTopLevel(syntaxTreeRoot);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("COMPILE EXECUTION TIME: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Debug("\n\nBytecode:\n{Bytecode}", topLvlChunk);

            // [Execution]==============================================================================================
            stepTimer?.Reset();
            totTimer?.Start();
            stepTimer?.Start();

            var topLvlReturnVal = VirtualMachine.ExecuteTopLevel(topLvlChunk);

            stepTimer?.Stop();
            totTimer?.Stop();

            logger?.Info("VIRTUAL MACHINE EXECUTION TIME: {Time}ms", stepTimer?.Elapsed.ToString(TimeFormat));
            logger?.Info("TOTAL EXECUTION TIME: {Time}ms", totTimer?.Elapsed.ToString(TimeFormat));
            return topLvlReturnVal;
        }
    }
}
