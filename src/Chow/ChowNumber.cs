using System;
using System.Globalization;
namespace Chow.Values
{
    public class ChowNumber : ChowValue
    {
        const int BoolTrueIntValue = 1;
        readonly double _val;

        public ChowNumber(double val)
        {
            _val = val;
        }

        public ChowNumber(int val)
        {
            _val = val;
        }

        public override ChowValueType Type => ChowValueType.Number;

        public override int ToInt() => (int)_val;

        public override double ToDouble() => _val;

        public override bool ToBool() => (int)_val == BoolTrueIntValue;

        public override bool IsType(ChowValueType type) => type == ChowValueType.Number;

        static double AsDouble(ChowValue v)
        {
            if (v is ChowNumber n) return n._val;
            if (v is ChowBool b) return b == new ChowBool(true) ? 1.0 : 0.0;

            throw new InvalidOperationException($"Cannot coerce {v.GetType().Name} to number.");
        }

        protected override ChowValue Add(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot add ChowNumber and {right.GetType().Name}.");

            return new ChowNumber(_val + AsDouble(right));
        }

        protected override ChowValue Subtract(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot subtract ChowNumber and {right.GetType().Name}.");

            return new ChowNumber(_val - AsDouble(right));
        }

        protected override ChowValue Multiply(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot multiply ChowNumber and {right.GetType().Name}.");

            return new ChowNumber(_val * AsDouble(right));
        }

        protected override ChowValue Divide(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot divide ChowNumber and {right.GetType().Name}.");

            return new ChowNumber(_val / AsDouble(right));
        }

        protected override bool EqualTo(ChowValue right)
        {
            if (right is ChowNumber rNum) return _val.Equals(rNum._val);
            if (right is ChowBool) return _val.Equals(AsDouble(right));

            return false;
        }

        protected override bool EqualTo(double right) => _val.Equals(right);

        protected override bool LessThan(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot compare ChowNumber with {right.GetType().Name}.");

            return _val < AsDouble(right);
        }

        protected override bool GreaterThan(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot compare ChowNumber with {right.GetType().Name}.");

            return _val > AsDouble(right);
        }

        protected override bool LessThanOrEqualTo(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot compare ChowNumber with {right.GetType().Name}.");

            return _val <= AsDouble(right);
        }

        protected override bool GreaterThanOrEqualTo(ChowValue right)
        {
            if (right is not ChowNumber && right is not ChowBool)
                throw new InvalidOperationException($"Cannot compare ChowNumber with {right.GetType().Name}.");

            return _val >= AsDouble(right);
        }

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val.ToString(CultureInfo.InvariantCulture);
    }
}
