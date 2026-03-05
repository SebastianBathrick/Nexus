using Nexus.Compilation;
using Nexus.Runtime;
using Nexus.Runtime.Values;
using Nexus.LexicalAnalysis;
using Nexus.LexicalAnalysis.Tokens;
using Nexus.SyntaxAnalysis;

namespace Nexus
{
    public static class NovaInterpreter
    {
        public static NexusValue Run(string sourceCode)
        {
            var tokenCollection = Lexer.Lex<TokenCollection>(sourceCode);
            Nexus.Logging.Log.Debug(tokenCollection.ToString());
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);
            Nexus.Logging.Log.Debug(syntaxTreeRoot.ToString());
            var entryPointChunk = Compiler.CompileFromSyntaxTree(syntaxTreeRoot);
            Nexus.Logging.Log.Debug(entryPointChunk.ToString());
            return VirtualMachine.ExecuteChunk(entryPointChunk);
        }
    }
}
