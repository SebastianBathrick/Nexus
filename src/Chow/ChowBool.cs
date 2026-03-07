using System;
namespace Chow.Values
{
    public class ChowBool : ChowValue
    {
        public const int TrueIntValue = 1;
        const int FalseIntValue = 0;
        public const double TrueValue = 1.0;
        public const double FalseValue = 0.0;


        readonly bool _val;

        public ChowBool(bool val)
        {
            _val = val;
        }

        public override ChowValueType Type => ChowValueType.Bool;

        public override int ToInt() => _val ? TrueIntValue : FalseIntValue;

        public override double ToDouble() => _val ? TrueValue : FalseValue;

        public override bool ToBool() => _val;

        public override bool IsType(ChowValueType type) => type == ChowValueType.Bool;

        ChowNumber AsNumber() => new ChowNumber(ToDouble());

        ChowValue RightAsNumber(ChowValue right) => right is ChowBool rightBool ? rightBool.AsNumber() : right;

        protected override ChowValue Add(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot add ChowBool and {right.GetType().Name}.");

            return AsNumber() + RightAsNumber(right);
        }

        protected override ChowValue Subtract(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot subtract ChowBool and {right.GetType().Name}.");

            return AsNumber() - RightAsNumber(right);
        }

        protected override ChowValue Multiply(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot multiply ChowBool and {right.GetType().Name}.");

            return AsNumber() * RightAsNumber(right);
        }

        protected override ChowValue Divide(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot divide ChowBool and {right.GetType().Name}.");

            return AsNumber() / RightAsNumber(right);
        }

        protected override bool EqualTo(ChowValue right)
        {
            if (right is ChowBool rightBool) return _val == rightBool._val;
            if (right is ChowNumber) return AsNumber() == right;

            return false;
        }

        protected override bool EqualTo(double right) => ToDouble().Equals(right);

        protected override bool LessThan(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot compare ChowBool and {right.GetType().Name}.");

            return AsNumber() < RightAsNumber(right);
        }

        protected override bool GreaterThan(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot compare ChowBool and {right.GetType().Name}.");

            return AsNumber() > RightAsNumber(right);
        }

        protected override bool LessThanOrEqualTo(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot compare ChowBool and {right.GetType().Name}.");

            return AsNumber() <= RightAsNumber(right);
        }

        protected override bool GreaterThanOrEqualTo(ChowValue right)
        {
            if (!(right is ChowNumber) && !(right is ChowBool))
                throw new InvalidOperationException($"Cannot compare ChowBool and {right.GetType().Name}.");

            return AsNumber() >= RightAsNumber(right);
        }

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val ? "true" : "false";
    }
}
