using Nexus.Compilation;
using Nexus.Execution;
using Nexus.Execution.Values;
using Nexus.Lexing;
using Nexus.Logging;
using Nexus.Parsing;

namespace Nexus
{
    public static class NexusInterpreter
    {
        public static NexusValue Run(string sourceCode)
        {
            var tokenCollection = Lexer.Lex(sourceCode);
            Log.Debug(tokenCollection.ToString());
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);
            Log.Debug(syntaxTreeRoot.ToString());
            var topLevel = ChunkCompiler.CompileTopLevel(syntaxTreeRoot);
            Log.Debug(topLevel.ToString());
            return VirtualMachine.ExecuteTopLevel(topLevel);
        }
    }
}
