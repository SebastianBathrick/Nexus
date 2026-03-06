using Nexus.Lexing;
using Nexus.Parsing;
using Nexus.Parsing.Expressions;
using Nexus.Parsing.Statements;

namespace Nexus.Tests;

[TestFixture]
public class ParserTests
{
    static Node Parse(string source)
    {
        // Use real TokenCollection from Nexus.Tokens (internal, visible via InternalsVisibleTo).
        var tokenCollection = Lexer.Lex(source);
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
    public void ParseTokens_AndBindsTighterThanOr_OrIsRootOperator()
    {
        // "true or false and true" should parse as "true or (false and true)"
        // so the root expression operator must be Or, not And
        var root = Parse("return true or false and true");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.LogicalOr));
        Assert.That(expr.Right, Is.InstanceOf<ExpressionNode>());
        Assert.That(((ExpressionNode)expr.Right).Operator, Is.EqualTo(ExpressionOperator.LogicalAnd));
    }

    [Test]
    public void ParseTokens_NotIsFactorLevel_AppliedBeforeArithmetic()
    {
        // "not true" → UnaryExpressionNode at factor level
        var root = Parse("return not true and false");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        // Root should be And; left should be UnaryExpressionNode(Not, true)
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.LogicalAnd));
        Assert.That(expr.Left, Is.InstanceOf<UnaryExpressionNode>());
        Assert.That(((UnaryExpressionNode)expr.Left).Operator, Is.EqualTo(ExpressionOperator.LogicalNot));
    }

    [Test]
    public void ParseTokens_DoubleNot_NestedUnaryNodes()
    {
        var root = Parse("return not not false");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var outer = (UnaryExpressionNode)returnNode.Expression;
        Assert.That(outer.Operator, Is.EqualTo(ExpressionOperator.LogicalNot));
        Assert.That(outer.Operand, Is.InstanceOf<UnaryExpressionNode>());
        Assert.That(((UnaryExpressionNode)outer.Operand).Operator, Is.EqualTo(ExpressionOperator.LogicalNot));
    }

    [Test]
    public void ParseTokens_ComparisonOperator_IsBelowArithmetic_ArithmeticIsLeaf()
    {
        // "1 + 2 < 3 + 4" → root is LessThan; both children are Addition nodes
        var root = Parse("return 1 + 2 < 3 + 4");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.LessThan));
        Assert.That(expr.Left, Is.InstanceOf<ExpressionNode>());
        Assert.That(((ExpressionNode)expr.Left).Operator, Is.EqualTo(ExpressionOperator.Addition));
        Assert.That(expr.Right, Is.InstanceOf<ExpressionNode>());
        Assert.That(((ExpressionNode)expr.Right).Operator, Is.EqualTo(ExpressionOperator.Addition));
    }

    [Test]
    public void ParseTokens_IsKeyword_ParsesAsEquality()
    {
        var root = Parse("return 1 is 1");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.Equality));
    }

    [Test]
    public void ParseTokens_IsNotKeyword_ParsesAsInequality()
    {
        var root = Parse("return 1 is not 2");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.Inequality));
    }

    [Test]
    public void ParseTokens_SymbolNotEqual_ParsesAsInequality()
    {
        var root = Parse("return 1 != 2");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.Inequality));
    }

    [Test]
    public void ParseTokens_NegatedParen_ParsesAsMultiplicationByNegativeOne()
    {
        // -(2 * 3) → ExpressionNode(Multiply, -1, ExpressionNode(Multiply, 2, 3))
        var root = Parse("return -(2 * 3)");
        var returnNode = (ReturnNode)((BlockNode)((SyntaxTree)root).TopLevelBlockNode).Statements[0];
        var expr = (ExpressionNode)returnNode.Expression;
        Assert.That(expr.Operator, Is.EqualTo(ExpressionOperator.Multiplication));
        Assert.That(expr.Left, Is.InstanceOf<NumberLiteralNode>());
        Assert.That(((NumberLiteralNode)expr.Left).GetNumberValue(), Is.EqualTo(-1));
        Assert.That(expr.Right, Is.InstanceOf<ExpressionNode>());
    }

    // Assignments

    [Test]
    public void ParseTokens_AssignNumberLiteral_ReturnsAssignmentNodeWithIdentifierAndLiteral()
    {
        var root = Parse("x = 42");
        var block = (BlockNode)((SyntaxTree)root).TopLevelBlockNode;
        Assert.That(block.Statements.Length, Is.EqualTo(1));
        var assignment = (AssignmentNode)block.Statements[0];
        Assert.That(assignment.Identifier, Is.EqualTo("x"));
        Assert.That(assignment.Expression, Is.InstanceOf<NumberLiteralNode>());
    }

    [Test]
    public void ParseTokens_AssignBoolLiteral_ReturnsAssignmentNodeWithBoolExpression()
    {
        var root = Parse("x = true");
        var block = (BlockNode)((SyntaxTree)root).TopLevelBlockNode;
        var assignment = (AssignmentNode)block.Statements[0];
        Assert.That(assignment.Identifier, Is.EqualTo("x"));
        Assert.That(assignment.Expression, Is.InstanceOf<BoolLiteralNode>());
    }

    [Test]
    public void ParseTokens_AssignArithmeticExpression_ReturnsAssignmentNodeWithExpressionNode()
    {
        var root = Parse("x = 1 + 2");
        var block = (BlockNode)((SyntaxTree)root).TopLevelBlockNode;
        var assignment = (AssignmentNode)block.Statements[0];
        Assert.That(assignment.Identifier, Is.EqualTo("x"));
        Assert.That(assignment.Expression, Is.InstanceOf<ExpressionNode>());
        Assert.That(((ExpressionNode)assignment.Expression).Operator, Is.EqualTo(ExpressionOperator.Addition));
    }

    [Test]
    public void ParseTokens_MultipleAssignments_ParsesAllStatements()
    {
        var root = Parse("x = 1\ny = 2");
        var block = (BlockNode)((SyntaxTree)root).TopLevelBlockNode;
        Assert.That(block.Statements.Length, Is.EqualTo(2));
        Assert.That(((AssignmentNode)block.Statements[0]).Identifier, Is.EqualTo("x"));
        Assert.That(((AssignmentNode)block.Statements[1]).Identifier, Is.EqualTo("y"));
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
