namespace Chow.Values
{
    /// <summary>
    ///     Represents a value in the ChowEngine programming language created during compilation and used
    ///     during execution. This is the base class for all values in the ChowEngine programming language.
    ///     This can represent a number, boolean, string, function, or table.
    /// </summary>
    public abstract class ChowValue
    {
        public abstract ChowValueType Type { get; }

        public abstract int ToInt();

        public abstract double ToDouble();

        public abstract bool ToBool();

        public abstract bool IsType(ChowValueType type);

        /// <summary>Returns the sum of this and <paramref name="right" />.</summary>
        protected abstract ChowValue Add(ChowValue right);

        /// <summary>Returns the difference of this and <paramref name="right" />.</summary>
        protected abstract ChowValue Subtract(ChowValue right);

        /// <summary>Returns the product of this and <paramref name="right" />.</summary>
        protected abstract ChowValue Multiply(ChowValue right);

        /// <summary>Returns the quotient of this and <paramref name="right" />.</summary>
        protected abstract ChowValue Divide(ChowValue right);

        /// <summary>Returns whether this is equal to <paramref name="right" />.</summary>
        protected abstract bool EqualTo(ChowValue right);

        /// <summary>Returns whether this is equal to <paramref name="right" />.</summary>
        protected abstract bool EqualTo(double right);

        /// <summary>Returns whether this is less than <paramref name="right" />.</summary>
        protected abstract bool LessThan(ChowValue right);

        /// <summary>Returns whether this is greater than <paramref name="right" />.</summary>
        protected abstract bool GreaterThan(ChowValue right);

        /// <summary>Returns whether this is less than or equal to <paramref name="right" />.</summary>
        protected abstract bool LessThanOrEqualTo(ChowValue right);

        /// <summary>Returns whether this is greater than or equal to <paramref name="right" />.</summary>
        protected abstract bool GreaterThanOrEqualTo(ChowValue right);

        /// <summary>Adds two values (uses left's <see cref="Add" />).</summary>
        public static ChowValue operator +(ChowValue left, ChowValue right) => left.Add(right);

        /// <summary>Subtracts two values (uses left's <see cref="Subtract" />).</summary>
        public static ChowValue operator -(ChowValue left, ChowValue right) => left.Subtract(right);

        /// <summary>Multiplies two values (uses left's <see cref="Multiply" />).</summary>
        public static ChowValue operator *(ChowValue left, ChowValue right) => left.Multiply(right);

        /// <summary>Divides two values (uses left's <see cref="Divide" />).</summary>
        public static ChowValue operator /(ChowValue left, ChowValue right) => left.Divide(right);

        /// <summary>Equality (uses left's <see cref="EqualTo(ChowValue)" />).</summary>
        public static bool operator ==(ChowValue left, ChowValue right) => left.EqualTo(right);

        /// <summary>Inequality (uses left's <see cref="EqualTo(ChowValue)" />).</summary>
        public static bool operator !=(ChowValue left, ChowValue right) => !left.EqualTo(right);

        public static bool operator ==(ChowValue left, double right) => left.EqualTo(right);

        public static bool operator !=(ChowValue left, double right) => !left.EqualTo(right);

        public static bool operator ==(double left, ChowValue right) => right.EqualTo(left);

        public static bool operator !=(double left, ChowValue right) => !right.EqualTo(left);

        /// <summary>Less-than (uses left's <see cref="LessThan" />).</summary>
        public static bool operator <(ChowValue left, ChowValue right) => left.LessThan(right);

        /// <summary>Greater-than (uses left's <see cref="GreaterThan" />).</summary>
        public static bool operator >(ChowValue left, ChowValue right) => left.GreaterThan(right);

        /// <summary>Less-than-or-equal (uses left's <see cref="LessThanOrEqualTo" />).</summary>
        public static bool operator <=(ChowValue left, ChowValue right) => left.LessThanOrEqualTo(right);

        /// <summary>Greater-than-or-equal (uses left's <see cref="GreaterThanOrEqualTo" />).</summary>
        public static bool operator >=(ChowValue left, ChowValue right) => left.GreaterThanOrEqualTo(right);

        public override bool Equals(object? obj) => obj is ChowValue other && EqualTo(other);

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}
