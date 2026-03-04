namespace Nexus.Execution.Values
{
    public abstract class CluaValue
    {
        /// <summary>
        ///     Calculates and returns the sum of this instance and <paramref name="right" />.
        /// </summary>
        /// <param name="right">The right-hand operand.</param>
        /// <returns> Sum of this instance &amp; <paramref name="right" /> as a new <see cref="CluaValue" /> instance. </returns>
        protected abstract CluaValue Add(CluaValue right);

        /// <summary>
        ///     Calculates and returns the difference of this instance and <paramref name="right" />.
        /// </summary>
        /// <param name="right">The right-hand operand.</param>
        /// <returns> Difference of this instance &amp; <paramref name="right" /> as a new <see cref="CluaValue" /> instance. </returns>
        protected abstract CluaValue Subtract(CluaValue right);

        /// <summary>
        ///     Calculates and returns the product of this instance and <paramref name="right" />.
        /// </summary>
        /// <param name="right">The right-hand operand.</param>
        /// <returns> Product of this instance &amp; <paramref name="right" /> as a new <see cref="CluaValue" /> instance. </returns>
        protected abstract CluaValue Multiply(CluaValue right);

        /// <summary>
        ///     Calculates and returns the quotient of this instance and <paramref name="right" />.
        /// </summary>
        /// <param name="right">The right-hand operand.</param>
        /// <returns> Quotient of this instance &amp; <paramref name="right" /> as a new <see cref="CluaValue" /> instance. </returns>
        protected abstract CluaValue Divide(CluaValue right);

        /// <summary>
        ///     Determines whether this instance is equal to <paramref name="right"/>.
        /// </summary>
        /// <param name="right">The right-hand operand.</param>
        /// <returns><c>true</c> if this instance is equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
        protected abstract bool EqualTo(CluaValue right);

        /// <summary>
        ///     Determines whether this instance is equal to <paramref name="right"/>.
        /// </summary>
        /// <param name="right">The right-hand operand.</param>
        /// <returns><c>true</c> if this instance is equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
        protected abstract bool EqualTo(double right);

        /// <summary>
        ///     Adds two <see cref="CluaValue" /> instances using the left operand's <see cref="Add" /> implementation.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns> Sum of <paramref name="left" /> &amp; <paramref name="right" /> as a new <see cref="CluaValue" /> instance. </returns>
        public static CluaValue operator +(CluaValue left, CluaValue right) => left.Add(right);

        /// <summary>
        ///     Subtracts two <see cref="CluaValue" /> instances using the left operand's <see cref="Subtract" /> implementation.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns>
        ///     Difference of <paramref name="left" /> &amp; <paramref name="right" /> as a new <see cref="CluaValue" />
        ///     instance.
        /// </returns>
        public static CluaValue operator -(CluaValue left, CluaValue right) => left.Subtract(right);

        /// <summary>
        ///     Multiplies two <see cref="CluaValue" /> instances using the left operand's <see cref="Multiply" /> implementation.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns>
        ///     Product of <paramref name="left" /> &amp; <paramref name="right" /> as a new <see cref="CluaValue" />
        ///     instance.
        /// </returns>
        public static CluaValue operator *(CluaValue left, CluaValue right) => left.Multiply(right);

        /// <summary>
        ///     Divides two <see cref="CluaValue" /> instances using the left operand's <see cref="Divide" /> implementation.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns>
        ///     Quotient of <paramref name="left" /> &amp; <paramref name="right" /> as a new <see cref="CluaValue" />
        ///     instance.
        /// </returns>
        public static CluaValue operator /(CluaValue left, CluaValue right) => left.Divide(right);

        /// <summary>
        ///     Compares two <see cref="CluaValue"/> instances for equality using the left operand's <see cref="EqualTo"/> implementation.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
        public static bool operator ==(CluaValue left, CluaValue right) => left.EqualTo(right);

        /// <summary>
        ///     Compares two <see cref="CluaValue"/> instances for inequality using the left operand's <see cref="EqualTo"/> implementation.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise <c>false</c>.</returns>
        public static bool operator !=(CluaValue left, CluaValue right) => !left.EqualTo(right);

        public static bool operator ==(CluaValue left, double right) => left.EqualTo(right);
        public static bool operator !=(CluaValue left, double right) => !left.EqualTo(right);

        public static bool operator ==(double left, CluaValue right) => right.EqualTo(left);
        public static bool operator !=(double left, CluaValue right) => !right.EqualTo(left);

        public override bool Equals(object? obj) => obj is CluaValue other && EqualTo(other);

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}
