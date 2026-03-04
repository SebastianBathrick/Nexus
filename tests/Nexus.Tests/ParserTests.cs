using System;
using Nexus.LexicalAnalysis;
using Nexus.SyntaxAnalysis;
using Nexus.SyntaxAnalysis.Expressions;
using Nexus.Tokens;
using NUnit.Framework;

namespace Nexus.Tests;

[TestFixture]
public class ParserTests
{
    static Node Parse(string source)
    {
        // Use real TokenCollection from Nexus.Tokens (internal, visible via InternalsVisibleTo).
        var tokenCollection = Lexer.Lex<Nexus.Tokens.TokenCollection>(source);
        return Parser.ParseTokens(tokenCollection);
    }

    [Test]
    public void ParseTokens_ReturnNumberLiteral_ReturnsSyntaxTreeWithReturnNode()
    {
        var root = Parse("return 42");
        Assert.That(root, Is.InstanceOf<SyntaxTree>());
        var tree = (SyntaxTree)root;
        Assert.That(tree.TopLevelBlockNode, Is.InstanceOf<BlockNode>());
        var block = (BlockNode)tree.TopLevelBlockNode;
        Assert.That(block.Statements.Length, Is.EqualTo(1));
        Assert.That(block.Statements[0], Is.InstanceOf<ReturnNode>());
        var returnNode = (ReturnNode)block.Statements[0];
        Assert.That(returnNode.Expression, Is.InstanceOf<NumberLiteralNode>());
    }

    [Test]
    public void ParseTokens_ReturnArithmeticExpression_ReturnsExpressionNodeWithAddition()
    {
        var root = Parse("return 1 + 2");
        var tree = (SyntaxTree)root;
        var block = (BlockNode)tree.TopLevelBlockNode;
        var returnNode = (ReturnNode)block.Statements[0];
        Assert.That(returnNode.Expression, Is.InstanceOf<ExpressionNode>());
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.Addition));
    }

    [Test]
    public void ParseTokens_ReturnComparison_ReturnsExpressionNodeWithComparisonOperator()
    {
        var root = Parse("return 1 < 2");
        var tree = (SyntaxTree)root;
        var block = (BlockNode)tree.TopLevelBlockNode;
        var returnNode = (ReturnNode)block.Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.LessThan));
    }

    [Test]
    public void ParseTokens_ReturnLogicalExpression_ReturnsExpressionNodeWithLogicalAnd()
    {
        var root = Parse("return true and false");
        var tree = (SyntaxTree)root;
        var block = (BlockNode)tree.TopLevelBlockNode;
        var returnNode = (ReturnNode)block.Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.LogicalAnd));
    }

    [Test]
    public void ParseTokens_ReturnNestedParentheses_ParsesSuccessfully()
    {
        var root = Parse("return (1 + 2) * 3");
        Assert.That(root, Is.InstanceOf<SyntaxTree>());
        var tree = (SyntaxTree)root;
        var block = (BlockNode)tree.TopLevelBlockNode;
        var returnNode = (ReturnNode)block.Statements[0];
        Assert.That(returnNode.Expression, Is.InstanceOf<ExpressionNode>());
    }

    [Test]
    public void ParseTokens_ReturnUnaryMinus_ParsesSuccessfully()
    {
        var root = Parse("return -5");
        Assert.That(root, Is.InstanceOf<SyntaxTree>());
        var tree = (SyntaxTree)root;
        var block = (BlockNode)tree.TopLevelBlockNode;
        var returnNode = (ReturnNode)block.Statements[0];
        Assert.That(returnNode.Expression, Is.InstanceOf<ExpressionNode>());
    }

    [Test]
    public void ParseTokens_ReturnNotLiteral_ParsesSuccessfully()
    {
        var root = Parse("return not true");
        var tree = (SyntaxTree)root;
        var block = (BlockNode)tree.TopLevelBlockNode;
        var returnNode = (ReturnNode)block.Statements[0];
        Assert.That(returnNode.Expression, Is.InstanceOf<UnaryExpressionNode>());
    }

    [Test]
    public void ParseTokens_UnexpectedTokenAfterExpression_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parse("return 1 2"));
    }

    [Test]
    public void ParseTokens_MissingClosingParen_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parse("return (1 + 2"));
    }

    [Test]
    public void ParseTokens_InvalidFactor_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parse("return * 1"));
    }
}
