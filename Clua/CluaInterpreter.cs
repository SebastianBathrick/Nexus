using Clua.Execution;
using Clua.LexicalAnalysis;
using Clua.SyntaxAnalysis;
using Clua.Compilation;
using Clua.Execution.Values;
using Clua.Tokens;

namespace Clua
{
    public static class CluaInterpreter
    {
        public static CluaValue Run(string sourceCode)
        {
            var tokenCollection = Lexer.Lex<TokenCollection>(sourceCode);
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);
            var entryPointChunk = Compiler.CompileFromSyntaxTree(syntaxTreeRoot);
            return Executor.ExecuteChunk(entryPointChunk);
        }
    }
}
