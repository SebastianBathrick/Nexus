using Nexus.Compilation;
using Nexus.Execution;
using Nexus.Execution.Values;
using Nexus.LexicalAnalysis;
using Nexus.SyntaxAnalysis;
using Nexus.Tokens;

namespace Nexus
{
    public static class CluaInterpreter
    {
        public static NexusValue Run(string sourceCode)
        {
            var tokenCollection = Lexer.Lex<TokenCollection>(sourceCode);
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);
            var entryPointChunk = Compiler.CompileFromSyntaxTree(syntaxTreeRoot);
            return Executor.ExecuteChunk(entryPointChunk);
        }
    }
}
