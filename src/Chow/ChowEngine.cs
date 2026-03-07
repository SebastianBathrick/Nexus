using System.Diagnostics;
using Chow.Compilation;
using Chow.Interpretation;
using Chow.Lexing;
using Chow.Logging;
using Chow.Parsing;
using Chow.Values;
namespace Chow
{
    public static class ChowEngine
    {
        const string TimeFormat = @"fff\.ffffff";
        const LogLevel DefaultMinimumLogLevel = LogLevel.Info;

        public static ChowValue Run<TLoggerType>(string sourceCode, LogLevel minLogLevel = DefaultMinimumLogLevel) where TLoggerType : ILogger, new() =>
            Run(sourceCode, new TLoggerType { MinimumLogLevel = minLogLevel });

        /// <summary>
        ///     Sequentially performs lexical analysis, syntax analysis, bytecode compilation, and then interpretation of the
        ///     provided source code.
        /// </summary>
        /// <param name="sourceCode">
        ///     Source code written in ChowEngine to compile into bytecode, which will be executed by the virtual machine
        ///     immediately after.
        /// </param>
        /// <param name="logger">
        ///     When provided, the logger object enables automatic execution time measurement, which is logged if
        ///     the logger’s <see cref="ILogger.MinimumLogLevel" /> is set to <see cref="LogLevel.Info" /> or higher.
        /// </param>
        /// <returns>
        ///     The ChowValue returned by the top-level code. If the code does not execute a top-level return statement with
        ///     an expression, then it will return a ChowNumber containing a <see cref="VirtualMachine.SuccessExitCode" />.
        /// </returns>
        public static ChowValue Run(string sourceCode, ILogger? logger = null)
        {
            // Measures individual execution times for each step (lexing, syntax analysis, compilation, execution)
            Stopwatch? stepTimer = null;

            // Measures total execution time for the entire pipeline 
            Stopwatch? totTimer = null;

            if (logger is not null && logger.MinimumLogLevel > LogLevel.None)
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
