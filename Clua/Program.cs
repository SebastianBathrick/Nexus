using Clua;
using Clua.LexicalAnalysis;
using Clua.Tokens;
using Clua.CodeGeneration;
using Clua.SyntaxAnalysis;

var tkns = Lexer.Lex<TokenCollection>("(1 + 2 * (5.3 - -3) - 10 / (2 * 8 + 4 - 3 * 80 * -(2-1.203))) / -2");
var ast = Parser.Parse(tkns);
var codeObj = new CodeObjectGenerator().Generate(ast);

Console.WriteLine(codeObj);

var res = VirtualMachine.ExecuteCode(codeObj);

Console.WriteLine(res);