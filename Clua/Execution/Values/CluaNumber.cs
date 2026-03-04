using System.Globalization;

namespace Clua.Execution.Values
{
    public class CluaNumber : CluaValue
    {
        readonly double _val;

        public CluaNumber(double val)
        {
            _val = val;
        }

        public CluaNumber(int val)
        {
            _val = val;
        }

        protected override CluaValue Add(CluaValue right)
        {
            if (right is not CluaNumber r) throw new InvalidOperationException($"Cannot add CluaNumber and {right.GetType().Name}.");

            return new CluaNumber(_val + r._val);
        }

        protected override CluaValue Subtract(CluaValue right)
        {
            if (right is not CluaNumber r) throw new InvalidOperationException($"Cannot subtract CluaNumber and {right.GetType().Name}.");

            return new CluaNumber(_val - r._val);
        }

        protected override CluaValue Multiply(CluaValue right)
        {
            if (right is not CluaNumber r) throw new InvalidOperationException($"Cannot multiply CluaNumber and {right.GetType().Name}.");

            return new CluaNumber(_val * r._val);
        }

        protected override CluaValue Divide(CluaValue right)
        {
            if (right is not CluaNumber r) throw new InvalidOperationException($"Cannot divide CluaNumber and {right.GetType().Name}.");

            return new CluaNumber(_val / r._val);
        }

        protected override bool EqualTo(CluaValue right) => right is CluaNumber rNum && _val.Equals(rNum._val);

        protected override bool EqualTo(double right) => _val.Equals(right);

        public override int GetHashCode() => _val.GetHashCode();

        public override string ToString() => _val.ToString(CultureInfo.InvariantCulture);
    }
}
