using Nexus.Runtime;
using Nexus.Runtime.Values;
namespace Nexus.Tests;

[TestFixture]
public class VirtualMachineTests
{
    [Test]
    public void ExecuteChunk_PushConstantThenReturn_ReturnsThatConstant()
    {
        var constants = new NexusValue[] { new NexusNumber(42) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void ExecuteChunk_PushTwoConstantsAddReturn_ReturnsSum()
    {
        var constants = new NexusValue[] { new NexusNumber(3), new NexusNumber(7) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.Add),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("10"));
    }

    [Test]
    public void ExecuteChunk_Subtract_ReturnsDifference()
    {
        var constants = new NexusValue[] { new NexusNumber(10), new NexusNumber(3) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.Subtract),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void ExecuteChunk_Multiply_ReturnsProduct()
    {
        var constants = new NexusValue[] { new NexusNumber(4), new NexusNumber(5) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.Multiply),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("20"));
    }

    [Test]
    public void ExecuteChunk_Divide_ReturnsQuotient()
    {
        var constants = new NexusValue[] { new NexusNumber(20), new NexusNumber(4) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.Divide),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void ExecuteChunk_EqualTo_ReturnsTrueWhenEqual()
    {
        var constants = new NexusValue[] { new NexusNumber(1), new NexusNumber(1) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.EqualTo),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result, Is.InstanceOf<NexusBool>());
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_NotEqualTo_ReturnsTrueWhenDifferent()
    {
        var constants = new NexusValue[] { new NexusNumber(1), new NexusNumber(2) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.NotEqualTo),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_LessThan_ReturnsBool()
    {
        var constants = new NexusValue[] { new NexusNumber(1), new NexusNumber(2) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.LessThan),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_GreaterThan_ReturnsBool()
    {
        var constants = new NexusValue[] { new NexusNumber(3), new NexusNumber(2) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.GreaterThan),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_And_ReturnsLogicalAnd()
    {
        var constants = new NexusValue[] { new NexusBool(true), new NexusBool(false) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.And),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void ExecuteChunk_Or_ReturnsLogicalOr()
    {
        var constants = new NexusValue[] { new NexusBool(true), new NexusBool(false) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 1),
            new Instruction(InstructionType.Or),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_Not_ReturnsNegatedBool()
    {
        var constants = new NexusValue[] { new NexusBool(false) };
        var instructions = new[]
        {
            new Instruction(InstructionType.PushConstant, CacheType.Constant, 0),
            new Instruction(InstructionType.Not),
            new Instruction(InstructionType.Return)
        };

        var chunk = new Chunk(instructions, constants);
        var result = VirtualMachine.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_UnknownOpType_ThrowsInvalidOperationException()
    {
        var instructions = new[] { new Instruction((InstructionType)999) };
        var chunk = new Chunk(instructions, Array.Empty<NexusValue>());
        Assert.Throws<InvalidOperationException>(() => VirtualMachine.ExecuteChunk(chunk));
    }
}
