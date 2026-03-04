using System;
using Nexus.Compilation;
using Nexus.Execution;
using Nexus.Execution.Values;
using Nexus.Operations;
using NUnit.Framework;

namespace Nexus.Tests;

[TestFixture]
public class ExecutorTests
{
    [Test]
    public void ExecuteChunk_PushConstantThenReturn_ReturnsThatConstant()
    {
        var constants = new NexusValue[] { new NexusNumber(42) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void ExecuteChunk_PushTwoConstantsAddReturn_ReturnsSum()
    {
        var constants = new NexusValue[] { new NexusNumber(3), new NexusNumber(7) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.Add),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("10"));
    }

    [Test]
    public void ExecuteChunk_Subtract_ReturnsDifference()
    {
        var constants = new NexusValue[] { new NexusNumber(10), new NexusNumber(3) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.Subtract),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void ExecuteChunk_Multiply_ReturnsProduct()
    {
        var constants = new NexusValue[] { new NexusNumber(4), new NexusNumber(5) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.Multiply),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("20"));
    }

    [Test]
    public void ExecuteChunk_Divide_ReturnsQuotient()
    {
        var constants = new NexusValue[] { new NexusNumber(20), new NexusNumber(4) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.Divide),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void ExecuteChunk_EqualTo_ReturnsTrueWhenEqual()
    {
        var constants = new NexusValue[] { new NexusNumber(1), new NexusNumber(1) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.EqualTo),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result, Is.InstanceOf<NexusBool>());
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_NotEqualTo_ReturnsTrueWhenDifferent()
    {
        var constants = new NexusValue[] { new NexusNumber(1), new NexusNumber(2) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.NotEqualTo),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_LessThan_ReturnsBool()
    {
        var constants = new NexusValue[] { new NexusNumber(1), new NexusNumber(2) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.LessThan),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_GreaterThan_ReturnsBool()
    {
        var constants = new NexusValue[] { new NexusNumber(3), new NexusNumber(2) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.GreaterThan),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_And_ReturnsLogicalAnd()
    {
        var constants = new NexusValue[] { new NexusBool(true), new NexusBool(false) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.And),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void ExecuteChunk_Or_ReturnsLogicalOr()
    {
        var constants = new NexusValue[] { new NexusBool(true), new NexusBool(false) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.PushConstant, CacheType.Constant, 1),
            new Op(OpType.Or),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_Not_ReturnsNegatedBool()
    {
        var constants = new NexusValue[] { new NexusBool(false) };
        var instructions = new[]
        {
            new Op(OpType.PushConstant, CacheType.Constant, 0),
            new Op(OpType.Not),
            new Op(OpType.Return)
        };
        var chunk = new Chunk(instructions, constants);
        var result = Executor.ExecuteChunk(chunk);
        Assert.That(result.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void ExecuteChunk_UnknownOpType_ThrowsInvalidOperationException()
    {
        var instructions = new[] { new Op((OpType)999) };
        var chunk = new Chunk(instructions, Array.Empty<NexusValue>());
        Assert.Throws<InvalidOperationException>(() => Executor.ExecuteChunk(chunk));
    }
}
