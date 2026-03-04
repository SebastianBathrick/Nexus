using Clua;
using Clua.Compilation;
using Clua.Execution;
using Clua.LexicalAnalysis;
using Clua.Tokens;
using Clua.SyntaxAnalysis;

var tkns = Lexer.Lex<TokenCollection>("return (1 + 2 * (5.3 - -3) - 10 / (2 * 8 + 4 - 3 * 80 * -(2-1.203))) / -2");
var root = Parser.ParseTokens(tkns);
var codeObj = Compiler.CompileFromSyntaxTree(root);

Console.WriteLine(codeObj);

var res = Executor.ExecuteChunk(codeObj);

Console.WriteLine(res);