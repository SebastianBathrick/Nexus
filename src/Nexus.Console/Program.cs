using Nexus;

var res =NovaInterpreter.Run("return (1 + 2 * (5.3 - -3) - 10 / (2 * 8 + 4 - 3 * 80 * -(2-1.203))) / -2");
Console.WriteLine(res);

res = NovaInterpreter.Run("return true and false or true");
Console.WriteLine(res);

res = NovaInterpreter.Run("return not true");
Console.WriteLine(res);

res = NovaInterpreter.Run("return true and 1");
Console.WriteLine(res);