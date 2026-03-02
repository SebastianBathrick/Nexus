using System.Globalization;
namespace Clua.SyntaxAnalysis.Expressions;

public class NumberLiteralNode(double val) : LiteralNode
{
    public override double GetNumberValue() => val;

    public override string ToString() => val.ToString(CultureInfo.InvariantCulture);
}
