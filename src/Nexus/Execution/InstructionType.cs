namespace Nexus.Runtime
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

        PushConstant,
        Declare,
        Assign,

        EnterScope,
        ExitScope,
    }
}
