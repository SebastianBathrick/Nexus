using System.Globalization;

namespace Clua.SyntaxAnalysis.Expressions
{
    class NumberLiteralNode : LiteralNode
    {
        readonly double _val;

        public NumberLiteralNode(double val)
        {
            _val = val;
        }

        public override double GetNumberValue() => _val;

        public override string ToString() => _val.ToString(CultureInfo.InvariantCulture);
    }
}
