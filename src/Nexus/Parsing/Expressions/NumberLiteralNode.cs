using System.Globalization;

namespace Nexus.Parsing.Expressions
{
    class NumberLiteralNode : LiteralNode
    {
        readonly double _val;

        public NumberLiteralNode(double val)
        {
            _val = val;
        }

        public override double GetNumberValue() => _val;

        internal override string ToDebugString(int depth) => $"{Pad(depth)}{nameof(NumberLiteralNode)}({_val.ToString(CultureInfo.InvariantCulture)})";
    }
}
