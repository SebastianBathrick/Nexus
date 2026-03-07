using Chow.Compilation;
using Chow.Interpretation;
using Chow.Values;
namespace Chow.Tests.Execution;

[TestFixture]
public class VirtualMachineTests
{
    [Test]
    public void ExecuteChunk_PushConstantThenReturn_ReturnsThatConstant()
    {
        var constants = new ChowValue[] { new ChowNumber(42) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void ExecuteChunk_PushTwoConstantsAddReturn_ReturnsSum()
    {
        var constants = new ChowValue[] { new ChowNumber(3), new ChowNumber(7) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.Add),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("10"));
    }

    [Test]
    public void ExecuteChunk_Subtract_ReturnsDifference()
    {
        var constants = new ChowValue[] { new ChowNumber(10), new ChowNumber(3) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.Subtract),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void ExecuteChunk_Multiply_ReturnsProduct()
    {
        var constants = new ChowValue[] { new ChowNumber(4), new ChowNumber(5) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.Multiply),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("20"));
    }

    [Test]
    public void ExecuteChunk_Divide_ReturnsQuotient()
    {
        var constants = new ChowValue[] { new ChowNumber(20), new ChowNumber(4) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.Divide),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void ExecuteChunk_EqualTo_ReturnsTrueWhenEqual()
    {
        var constants = new ChowValue[] { new ChowNumber(1), new ChowNumber(1) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.EqualTo),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result, Is.InstanceOf<ChowBool>());
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_NotEqualTo_ReturnsTrueWhenDifferent()
    {
        var constants = new ChowValue[] { new ChowNumber(1), new ChowNumber(2) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.NotEqualTo),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_LessThan_ReturnsBool()
    {
        var constants = new ChowValue[] { new ChowNumber(1), new ChowNumber(2) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.LessThan),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_GreaterThan_ReturnsBool()
    {
        var constants = new ChowValue[] { new ChowNumber(3), new ChowNumber(2) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.GreaterThan),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_And_ReturnsLogicalAnd()
    {
        var constants = new ChowValue[] { new ChowBool(true), new ChowBool(false) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.And),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void ExecuteChunk_Or_ReturnsLogicalOr()
    {
        var constants = new ChowValue[] { new ChowBool(true), new ChowBool(false) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.ConstantPush,1),
            new Instruction(InstructionType.Or),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_Not_ReturnsNegatedBool()
    {
        var constants = new ChowValue[] { new ChowBool(false) };
        var instructions = new[]
        {
            new Instruction(InstructionType.EnterScope),
            new Instruction(InstructionType.ConstantPush,0),
            new Instruction(InstructionType.Not),
            new Instruction(InstructionType.Return),
            new Instruction(InstructionType.ExitScope)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteTopLevel(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_UnknownOpType_ThrowsInvalidOperationException()
    {
        var instructions = new[] { new Instruction((InstructionType)999) };
        var chunk = new Chunk(instructions, Array.Empty<ChowValue>());
        Assert.Throws<InvalidOperationException>(() => VirtualMachine.ExecuteTopLevel(chunk));
    }
}
