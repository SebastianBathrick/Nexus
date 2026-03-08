using System;
using System.Globalization;

namespace Chow.Interpretation
{
    struct TaggedUnion
    {
        #region Constants

        const string BooleanTrueString = "true";
        const string BooleanFalseString = "false";
        const double BooleanTrueNumberValue = 1;
        const double BooleanFalseValue = 0;
        const double DoubleTolerance = 0.00000001;
        const double NotTagNumberValue = 0;
        const bool NotTagBoolValue = false;

        #endregion

        #region Properties

        public TagType Tag { get; }
        public bool BoolValue { get; set; }
        public double NumberValue { get; set; }

        private string _stringValue;

        public string StringValue
        {
            get => _stringValue ?? string.Empty;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _stringValue = value;
            }
        }

        #endregion

        #region Constructors

        public TaggedUnion(bool value)
        {
            Tag = TagType.Bool;
            BoolValue = value;
            NumberValue = NotTagNumberValue;
            _stringValue = null;
        }

        public TaggedUnion(double value)
        {
            Tag = TagType.Number;
            BoolValue = NotTagBoolValue;
            NumberValue = value;
            _stringValue = null;
        }

        public TaggedUnion(string value)
        {
            Tag = TagType.String;
            BoolValue = NotTagBoolValue;
            NumberValue = NotTagNumberValue;
            _stringValue = null;
            StringValue = value;
        }

        #endregion

        #region Coercion Methods

        bool CoercedToBool()
        {
            switch (Tag)
            {
                case TagType.Bool:
                    return BoolValue;
                case TagType.Number:
                    return Math.Abs(NumberValue - BooleanTrueNumberValue) < DoubleTolerance;
                case TagType.String:
                    return StringValue == BooleanTrueString;
                default:
                    throw new InvalidOperationException($"Invalid tag type: {Tag}");
            }
        }

        double CoercedToNumber()
        {
            switch (Tag)
            {
                case TagType.Bool:
                    return BoolValue ? BooleanTrueNumberValue : BooleanFalseValue;
                case TagType.Number:
                    return NumberValue;
                case TagType.String:
                    return double.Parse(StringValue);
                default:
                    throw new InvalidOperationException($"Invalid tag type: {Tag}");
            }
        }

        string CoercedToString()
        {
            switch (Tag)
            {
                case TagType.Bool:
                    return BoolValue ? BooleanTrueString : BooleanFalseString;
                case TagType.Number:
                    return NumberValue.ToString(CultureInfo.InvariantCulture);
                case TagType.String:
                    return _stringValue ?? string.Empty;
                default:
                    throw new InvalidOperationException($"Invalid tag type: {Tag}");
            }
        }

        #endregion

        #region IsTruthy

        static bool IsTruthy(TaggedUnion v)
        {
            switch (v.Tag)
            {
                case TagType.Bool:
                    return v.BoolValue;
                case TagType.Number:
                    return Math.Abs(v.NumberValue) >= DoubleTolerance;
                default:
                    throw new InvalidOperationException($"IsTruthy not supported for tag type: {v.Tag}");
            }
        }

        #endregion

        #region Arithmetic Operators

        static void RequireNumericOrBool(TaggedUnion a, TaggedUnion b, string op)
        {
            if ((a.Tag != TagType.Number && a.Tag != TagType.Bool) ||
                (b.Tag != TagType.Number && b.Tag != TagType.Bool))
                throw new InvalidOperationException(
                    $"Cannot {op} {a.Tag} and {b.Tag}.");
        }

        public static TaggedUnion operator +(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "add");
            return new TaggedUnion(a.CoercedToNumber() + b.CoercedToNumber());
        }

        public static TaggedUnion operator -(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "subtract");
            return new TaggedUnion(a.CoercedToNumber() - b.CoercedToNumber());
        }

        public static TaggedUnion operator *(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "multiply");
            return new TaggedUnion(a.CoercedToNumber() * b.CoercedToNumber());
        }

        public static TaggedUnion operator /(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "divide");
            return new TaggedUnion(a.CoercedToNumber() / b.CoercedToNumber());
        }

        #endregion

        #region Comparison Operators

        public static TaggedUnion operator <(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "compare");
            return new TaggedUnion(a.CoercedToNumber() < b.CoercedToNumber());
        }

        public static TaggedUnion operator >(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "compare");
            return new TaggedUnion(a.CoercedToNumber() > b.CoercedToNumber());
        }

        public static TaggedUnion operator <=(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "compare");
            return new TaggedUnion(a.CoercedToNumber() <= b.CoercedToNumber());
        }

        public static TaggedUnion operator >=(TaggedUnion a, TaggedUnion b)
        {
            RequireNumericOrBool(a, b, "compare");
            return new TaggedUnion(a.CoercedToNumber() >= b.CoercedToNumber());
        }

        #endregion

        #region Equality Operators

        public static TaggedUnion operator ==(TaggedUnion a, TaggedUnion b)
        {
            if (a.Tag == TagType.Number && b.Tag == TagType.Number)
                return new TaggedUnion(a.NumberValue.Equals(b.NumberValue));
            if (a.Tag == TagType.Bool && b.Tag == TagType.Bool)
                return new TaggedUnion(a.BoolValue == b.BoolValue);
            if ((a.Tag == TagType.Number && b.Tag == TagType.Bool) ||
                (a.Tag == TagType.Bool && b.Tag == TagType.Number))
                return new TaggedUnion(a.CoercedToNumber().Equals(b.CoercedToNumber()));
            return new TaggedUnion(false);
        }

        public static TaggedUnion operator !=(TaggedUnion a, TaggedUnion b)
        {
            TaggedUnion eq = a == b;
            return new TaggedUnion(!eq.BoolValue);
        }

        public override bool Equals(object obj) => obj is TaggedUnion other && (this == other).BoolValue;

        public override int GetHashCode()
        {
            switch (Tag)
            {
                case TagType.Bool:   return BoolValue.GetHashCode();
                case TagType.Number: return NumberValue.GetHashCode();
                case TagType.String: return (_stringValue ?? string.Empty).GetHashCode();
                default:             throw new InvalidOperationException($"Invalid tag type: {Tag}");
            }
        }

        #endregion

        #region Logical Methods

        public static TaggedUnion And(TaggedUnion a, TaggedUnion b) =>
            new TaggedUnion(IsTruthy(a) && IsTruthy(b));

        public static TaggedUnion Or(TaggedUnion a, TaggedUnion b) =>
            new TaggedUnion(IsTruthy(a) || IsTruthy(b));

        public static TaggedUnion Not(TaggedUnion a) =>
            new TaggedUnion(!IsTruthy(a));

        #endregion

        #region ToString

        public override string ToString()
        {
            switch (Tag)
            {
                case TagType.Bool:   return BoolValue ? BooleanTrueString : BooleanFalseString;
                case TagType.Number: return NumberValue.ToString(CultureInfo.InvariantCulture);
                case TagType.String: return _stringValue ?? string.Empty;
                default:             throw new InvalidOperationException($"Invalid tag type: {Tag}");
            }
        }

        #endregion
    }

    enum TagType { Bool, Number, String }
}
