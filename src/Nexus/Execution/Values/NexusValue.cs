namespace Nexus.Execution.Values
{
    /// <summary>
    ///     Represents a value in the Nexus programming language created during compilation and used
    ///     during execution. This is the base class for all values in the Nexus programming language.
    ///     This can represent a number, boolean, string, function, or table.
    /// </summary>
    public abstract class NexusValue
    {
        public abstract int ToInt();
        public abstract double ToDouble();
        public abstract bool ToBool();

        public abstract bool IsType(NexusValueType type);
        public abstract NexusValueType Type { get; }

        /// <summary>Returns the sum of this and <paramref name="right" />.</summary>
        protected abstract NexusValue Add(NexusValue right);

        /// <summary>Returns the difference of this and <paramref name="right" />.</summary>
        protected abstract NexusValue Subtract(NexusValue right);

        /// <summary>Returns the product of this and <paramref name="right" />.</summary>
        protected abstract NexusValue Multiply(NexusValue right);

        /// <summary>Returns the quotient of this and <paramref name="right" />.</summary>
        protected abstract NexusValue Divide(NexusValue right);

        /// <summary>Returns whether this is equal to <paramref name="right" />.</summary>
        protected abstract bool EqualTo(NexusValue right);

        /// <summary>Returns whether this is equal to <paramref name="right" />.</summary>
        protected abstract bool EqualTo(double right);

        /// <summary>Returns whether this is less than <paramref name="right" />.</summary>
        protected abstract bool LessThan(NexusValue right);

        /// <summary>Returns whether this is greater than <paramref name="right" />.</summary>
        protected abstract bool GreaterThan(NexusValue right);

        /// <summary>Returns whether this is less than or equal to <paramref name="right" />.</summary>
        protected abstract bool LessThanOrEqualTo(NexusValue right);

        /// <summary>Returns whether this is greater than or equal to <paramref name="right" />.</summary>
        protected abstract bool GreaterThanOrEqualTo(NexusValue right);

        /// <summary>Adds two values (uses left's <see cref="Add" />).</summary>
        public static NexusValue operator +(NexusValue left, NexusValue right) => left.Add(right);

        /// <summary>Subtracts two values (uses left's <see cref="Subtract" />).</summary>
        public static NexusValue operator -(NexusValue left, NexusValue right) => left.Subtract(right);

        /// <summary>Multiplies two values (uses left's <see cref="Multiply" />).</summary>
        public static NexusValue operator *(NexusValue left, NexusValue right) => left.Multiply(right);

        /// <summary>Divides two values (uses left's <see cref="Divide" />).</summary>
        public static NexusValue operator /(NexusValue left, NexusValue right) => left.Divide(right);

        /// <summary>Equality (uses left's <see cref="EqualTo(NexusValue)" />).</summary>
        public static bool operator ==(NexusValue left, NexusValue right) => left.EqualTo(right);

        /// <summary>Inequality (uses left's <see cref="EqualTo(NexusValue)" />).</summary>
        public static bool operator !=(NexusValue left, NexusValue right) => !left.EqualTo(right);

        public static bool operator ==(NexusValue left, double right) => left.EqualTo(right);

        public static bool operator !=(NexusValue left, double right) => !left.EqualTo(right);

        public static bool operator ==(double left, NexusValue right) => right.EqualTo(left);

        public static bool operator !=(double left, NexusValue right) => !right.EqualTo(left);

        /// <summary>Less-than (uses left's <see cref="LessThan" />).</summary>
        public static bool operator <(NexusValue left, NexusValue right) => left.LessThan(right);

        /// <summary>Greater-than (uses left's <see cref="GreaterThan" />).</summary>
        public static bool operator >(NexusValue left, NexusValue right) => left.GreaterThan(right);

        /// <summary>Less-than-or-equal (uses left's <see cref="LessThanOrEqualTo" />).</summary>
        public static bool operator <=(NexusValue left, NexusValue right) => left.LessThanOrEqualTo(right);

        /// <summary>Greater-than-or-equal (uses left's <see cref="GreaterThanOrEqualTo" />).</summary>
        public static bool operator >=(NexusValue left, NexusValue right) => left.GreaterThanOrEqualTo(right);

        public override bool Equals(object? obj) => obj is NexusValue other && EqualTo(other);
        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}
