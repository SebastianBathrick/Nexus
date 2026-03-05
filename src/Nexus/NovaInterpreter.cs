using Nexus.Compilation;
using Nexus.Runtime;
using Nexus.Runtime.Values;
using Nexus.LexicalAnalysis;
using Nexus.LexicalAnalysis.Tokens;
using Nexus.SyntaxAnalysis;
using System;

namespace Nexus
{
    public static class NovaInterpreter
    {
        public static NexusValue Run(string sourceCode)
        {
            var tokenCollection = Lexer.Lex<TokenCollection>(sourceCode);
            Console.WriteLine(tokenCollection.ToString());
            var syntaxTreeRoot = Parser.ParseTokens(tokenCollection);
            Console.WriteLine(syntaxTreeRoot.ToString());
            var entryPointChunk = Compiler.CompileFromSyntaxTree(syntaxTreeRoot);
            Console.WriteLine(entryPointChunk.ToString());
            return VirtualMachine.ExecuteChunk(entryPointChunk);
            Console.WriteLine(result.ToString());
            return result;
        }
    }
}
