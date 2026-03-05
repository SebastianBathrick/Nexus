using System;
using Nexus.Compilation;
using Nexus.Runtime;
using Nexus.Runtime.Values;
using Nexus.SyntaxAnalysis;
using Nexus.SyntaxAnalysis.Expressions;
using Nexus.SyntaxAnalysis.Statements;
using NUnit.Framework;

namespace Nexus.Tests;

[TestFixture]
public class CompilerTests
{
    [Test]
    public void CompileFromSyntaxTree_ReturnNumberLiteral_ProducesPushConstantAndReturn()
    {
        var expr = new NumberLiteralNode(42);
        var returnNode = new ReturnNode(expr);
        var block = new BlockNode(new Node[] { returnNode });
        var tree = new SyntaxTree(block);

        var chunk = Compiler.CompileFromSyntaxTree(tree);

        Assert.That(chunk.Length, Is.EqualTo(2));
        Assert.That(chunk[0].OpType, Is.EqualTo(InstructionType.PushConstant));
        Assert.That(chunk[0].CacheIndex, Is.EqualTo(0));
        Assert.That(chunk[1].OpType, Is.EqualTo(InstructionType.Return));
        Assert.That(chunk.GetConstant(0).ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void CompileFromSyntaxTree_ReturnArithmeticExpression_ProducesPushPushAddReturn()
    {
        var left = new NumberLiteralNode(1);
        var right = new NumberLiteralNode(2);
        var expr = new ExpressionNode(ExpressionOperator.Addition, left, right);
        var returnNode = new ReturnNode(expr);
        var block = new BlockNode(new Node[] { returnNode });
        var tree = new SyntaxTree(block);

        var chunk = Compiler.CompileFromSyntaxTree(tree);

        Assert.That(chunk.Length, Is.EqualTo(4));
        Assert.That(chunk[0].OpType, Is.EqualTo(InstructionType.PushConstant));
        Assert.That(chunk[1].OpType, Is.EqualTo(InstructionType.PushConstant));
        Assert.That(chunk[2].OpType, Is.EqualTo(InstructionType.Add));
        Assert.That(chunk[3].OpType, Is.EqualTo(InstructionType.Return));
    }

    [Test]
    public void CompileFromSyntaxTree_ReturnComparison_ProducesComparisonOpAndReturn()
    {
        var left = new NumberLiteralNode(1);
        var right = new NumberLiteralNode(2);
        var expr = new ExpressionNode(ExpressionOperator.LessThan, left, right);
        var returnNode = new ReturnNode(expr);
        var block = new BlockNode(new Node[] { returnNode });
        var tree = new SyntaxTree(block);

        var chunk = Compiler.CompileFromSyntaxTree(tree);

        Assert.That(chunk.Length, Is.EqualTo(4));
        Assert.That(chunk[2].OpType, Is.EqualTo(InstructionType.LessThan));
        Assert.That(chunk[3].OpType, Is.EqualTo(InstructionType.Return));
    }

    [Test]
    public void CompileFromSyntaxTree_ReturnLogicalAnd_ProducesAndOpAndReturn()
    {
        var left = new BoolLiteralNode(true);
        var right = new BoolLiteralNode(false);
        var expr = new ExpressionNode(ExpressionOperator.LogicalAnd, left, right);
        var returnNode = new ReturnNode(expr);
        var block = new BlockNode(new Node[] { returnNode });
        var tree = new SyntaxTree(block);

        var chunk = Compiler.CompileFromSyntaxTree(tree);

        Assert.That(chunk.Length, Is.EqualTo(4));
        Assert.That(chunk[2].OpType, Is.EqualTo(InstructionType.And));
        Assert.That(chunk[3].OpType, Is.EqualTo(InstructionType.Return));
    }

    [Test]
    public void CompileFromSyntaxTree_NonSyntaxTreeRoot_ThrowsArgumentException()
    {
        var block = new BlockNode(Array.Empty<Node>());
        Assert.Throws<ArgumentException>(() => Compiler.CompileFromSyntaxTree(block));
    }
}
