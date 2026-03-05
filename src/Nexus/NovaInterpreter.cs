using Nexus.Compilation;
using Nexus.LexicalAnalysis;
using Nexus.Logging;
using Nexus.Runtime;
using Nexus.Runtime.Values;
using Nexus.SyntaxAnalysis;
namespace Nexus
{
    public static class NovaInterpreter
    {
        public static NexusValue Run(string sourceCode)
        {
            var tokenCollection = Lexer.Lex(sourceCode);
            Log.Debug(tokenCollection.ToString());
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);
            Log.Debug(syntaxTreeRoot.ToString());
            var entryPointChunk = new Compiler().CompileFromSyntaxTree(syntaxTreeRoot);
            Log.Debug(entryPointChunk.ToString());
            return VirtualMachine.ExecuteChunk(entryPointChunk);
        }
    }
}
