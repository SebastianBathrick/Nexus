using System.Globalization;
namespace Clua.AbstractSyntaxTree;

public class NumberLiteralNode(double val) : LiteralNode
{
    public override double GetNumberValue() => val;

    public override string ToString() => val.ToString(CultureInfo.InvariantCulture);
}
