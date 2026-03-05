using System.Globalization;
using System;

namespace Nexus.Runtime.Values
{
    public class NexusNumber : NexusValue
    {
        readonly double _val;

        public NexusNumber(double val)
        {
            _val = val;
        }

        public NexusNumber(int val)
        {
            _val = val;
        }

        static double AsDouble(NexusValue v)
        {
            if (v is NexusNumber n) return n._val;
            if (v is NexusBool b) return b == new NexusBool(true) ? 1.0 : 0.0;
            throw new InvalidOperationException($"Cannot coerce {v.GetType().Name} to number.");
        }

        protected override NexusValue Add(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot add NexusNumber and {right.GetType().Name}.");
            return new NexusNumber(_val + AsDouble(right));
        }

        protected override NexusValue Subtract(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot subtract NexusNumber and {right.GetType().Name}.");
            return new NexusNumber(_val - AsDouble(right));
        }

        protected override NexusValue Multiply(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot multiply NexusNumber and {right.GetType().Name}.");
            return new NexusNumber(_val * AsDouble(right));
        }

        protected override NexusValue Divide(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot divide NexusNumber and {right.GetType().Name}.");
            return new NexusNumber(_val / AsDouble(right));
        }

        protected override bool EqualTo(NexusValue right)
        {
            if (right is NexusNumber rNum) return _val.Equals(rNum._val);
            if (right is NexusBool) return _val.Equals(AsDouble(right));
            return false;
        }

        protected override bool EqualTo(double right) => _val.Equals(right);

        protected override bool LessThan(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val < AsDouble(right);
        }

        protected override bool GreaterThan(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val > AsDouble(right);
        }

        protected override bool LessThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val <= AsDouble(right);
        }

        protected override bool GreaterThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val >= AsDouble(right);
        }

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val.ToString(CultureInfo.InvariantCulture);
    }
}
