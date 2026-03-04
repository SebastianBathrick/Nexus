using System.Globalization;
using System;

namespace Nexus.Execution.Values
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

        protected override NexusValue Add(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot add CluaNumber and {right.GetType().Name}.");

            return new NexusNumber(_val + r._val);
        }

        protected override NexusValue Subtract(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot subtract CluaNumber and {right.GetType().Name}.");

            return new NexusNumber(_val - r._val);
        }

        protected override NexusValue Multiply(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot multiply CluaNumber and {right.GetType().Name}.");

            return new NexusNumber(_val * r._val);
        }

        protected override NexusValue Divide(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot divide CluaNumber and {right.GetType().Name}.");

            return new NexusNumber(_val / r._val);
        }

        protected override bool EqualTo(NexusValue right) => right is NexusNumber rNum && _val.Equals(rNum._val);

        protected override bool EqualTo(double right) => _val.Equals(right);

        protected override bool LessThan(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val < r._val;
        }

        protected override bool GreaterThan(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val > r._val;
        }

        protected override bool LessThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val <= r._val;
        }

        protected override bool GreaterThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber r) throw new InvalidOperationException($"Cannot compare NexusNumber with {right.GetType().Name}.");
            return _val >= r._val;
        }

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val.ToString(CultureInfo.InvariantCulture);
    }
}
