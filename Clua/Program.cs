using Clua;
using Clua.LexicalAnalysis;
using Clua.Tokens;

var tkns = Lexer.Lex<TokenCollection>("(1 + 2 * (5.3 - -3) - 10 / (2 * 8 + 4 - 3 * 80 * -(2-1.203))) / -2");
var ast = Parser.Parse(tkns);

Console.WriteLine(ast);