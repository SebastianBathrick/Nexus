namespace Chow.Compilation
{
    public enum InstructionType : uint
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Return,

        EqualTo,
        NotEqualTo,
        LessThan,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,

        And,
        Or,
        Not,

        LookupValue,

        PushConstant,
        Declare,
        Assign,

        EnterScope,
        ExitScope,
    }
}
