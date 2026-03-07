using System;

namespace Nexus.Execution.Values
{
    public class NexusBool : NexusValue
    {
        public const int TrueIntValue = 1;
        const int FalseIntValue = 0;
        public const double TrueValue = 1.0;
        public const double FalseValue = 0.0;


        readonly bool _val;

        public NexusBool(bool val)
        {
            _val = val;
        }

        public override int ToInt() => _val ? TrueIntValue : FalseIntValue;
        public override double ToDouble() => _val ? TrueValue : FalseValue;
        public override bool ToBool() => _val;

        public override bool IsType(NexusValueType type) => type == NexusValueType.Bool;
        public override NexusValueType Type => NexusValueType.Bool;

        NexusNumber AsNumber() => new(ToDouble());

        NexusValue RightAsNumber(NexusValue right) => right is NexusBool rightBool ? rightBool.AsNumber() : right;

        protected override NexusValue Add(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot add NexusBool and {right.GetType().Name}.");

            return AsNumber() + RightAsNumber(right);
        }

        protected override NexusValue Subtract(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot subtract NexusBool and {right.GetType().Name}.");

            return AsNumber() - RightAsNumber(right);
        }

        protected override NexusValue Multiply(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot multiply NexusBool and {right.GetType().Name}.");

            return AsNumber() * RightAsNumber(right);
        }

        protected override NexusValue Divide(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot divide NexusBool and {right.GetType().Name}.");

            return AsNumber() / RightAsNumber(right);
        }

        protected override bool EqualTo(NexusValue right)
        {
            if (right is NexusBool rightBool) return _val == rightBool._val;
            if (right is NexusNumber) return AsNumber() == right;

            return false;
        }

        protected override bool EqualTo(double right) => ToDouble().Equals(right);

        protected override bool LessThan(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");

            return AsNumber() < RightAsNumber(right);
        }

        protected override bool GreaterThan(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");

            return AsNumber() > RightAsNumber(right);
        }

        protected override bool LessThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");

            return AsNumber() <= RightAsNumber(right);
        }

        protected override bool GreaterThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");

            return AsNumber() >= RightAsNumber(right);
        }

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val ? "true" : "false";
    }
}
