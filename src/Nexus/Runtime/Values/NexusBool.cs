namespace Nexus.Runtime.Values
{
    public class NexusBool : NexusValue
    {
        readonly bool _val;

        public NexusBool(bool val)
        {
            _val = val;
        }

        double ToDouble() => _val ? 1.0 : 0.0;

        NexusNumber AsNumber() => new NexusNumber(ToDouble());

        NexusValue RightAsNumber(NexusValue right) => right is NexusBool rightBool ? rightBool.AsNumber() : right;

        protected override NexusValue Add(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot add NexusBool and {right.GetType().Name}.");

            return AsNumber() + RightAsNumber(right);
        }

        protected override NexusValue Subtract(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot subtract NexusBool and {right.GetType().Name}.");

            return AsNumber() - RightAsNumber(right);
        }

        protected override NexusValue Multiply(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot multiply NexusBool and {right.GetType().Name}.");

            return AsNumber() * RightAsNumber(right);
        }

        protected override NexusValue Divide(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot divide NexusBool and {right.GetType().Name}.");

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
                throw new System.InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");
            return AsNumber() < RightAsNumber(right);
        }

        protected override bool GreaterThan(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");
            return AsNumber() > RightAsNumber(right);
        }

        protected override bool LessThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");
            return AsNumber() <= RightAsNumber(right);
        }

        protected override bool GreaterThanOrEqualTo(NexusValue right)
        {
            if (right is not NexusNumber && right is not NexusBool)
                throw new System.InvalidOperationException($"Cannot compare NexusBool and {right.GetType().Name}.");
            
            return AsNumber() >= RightAsNumber(right);
        }

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val ? "true" : "false";
    }
}
