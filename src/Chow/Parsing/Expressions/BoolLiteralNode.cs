namespace Chow.Parsing.Expressions
{
    class BoolLiteralNode : LiteralNode
    {
        readonly bool _val;

        public BoolLiteralNode(bool val)
        {
            _val = val;
        }

        public bool GetBoolValue() => _val;

        public override double GetNumberValue() => _val ? 1.0 : 0.0;

        internal override string ToDebugString(int depth) => $"{Pad(depth)}{nameof(BoolLiteralNode)}({(_val ? "true" : "false")})";
    }
}
