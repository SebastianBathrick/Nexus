using Clua.Execution.Values;
using Clua.SyntaxAnalysis;
using Clua.SyntaxAnalysis.Expressions;
namespace Clua.Chunks.Generation;

static class ChunkGenerator
{
    public static Chunk GenerateTopLevelChunk(Node root)
    {
        if (root is not SyntaxTree tree)
            throw new ArgumentException($"{nameof(root)} is not a {nameof(SyntaxTree)}");

        if (tree.TopLevelBlockNode is not BlockNode block)
            throw new ArgumentException($"{nameof(tree.TopLevelBlockNode)} is not a {nameof(BlockNode)}");

        // TODO: Add functions, control flow constructs, and more statements.
        var tempData = new TempChunkCache();
        GetReturnInstructions(block.Statements.First(), tempData);
        return new Chunk(tempData.Instructions.ToArray(), tempData.Values.ToArray());
    }

    static void GetReturnInstructions(Node node, TempChunkCache tempCache)
    {
        if (node is not ReturnNode returnNode)
            throw new ArgumentException("node is not a ReturnNode");

        GetExpressionInstructions(returnNode.Expression, tempCache);
        tempCache.Instructions.Add(new Op(OpType.Return));
    }

    static void GetExpressionInstructions(Node node, TempChunkCache tempCache)
    {
        if (node is not ExpressionNode expression)
        {
            GetPushInstruction(node, tempCache);
            return;
        }

        GetExpressionInstructions(expression.Left, tempCache);
        GetExpressionInstructions(expression.Right, tempCache);
        tempCache.Instructions.Add(new Op(GetOpType(expression.Operator)));
    }

    static void GetPushInstruction(Node node, TempChunkCache tempCache)
    {
        // TODO: Add comparison and logic operators
        if (node is not NumberLiteralNode numNode)
            throw new ArgumentException("node is not a NumberLiteralNode");

        var numVal = numNode.GetNumberValue();
        var constIndex = tempCache.Values.FindIndex(x => x == numVal);

        if (constIndex == -1)
        {
            tempCache.Values.Add(new CluaNumber(numVal));
            constIndex = tempCache.Values.Count - 1;
        }

        tempCache.Instructions.Add(new Op(OpType.PushConstant, CacheType.Constant, constIndex));
    }

    static OpType GetOpType(ExpressionOperator op)
    {
        return op switch
        {
            ExpressionOperator.Addition => OpType.Add,
            ExpressionOperator.Subtraction => OpType.Subtract,
            ExpressionOperator.Multiplication => OpType.Multiply,
            ExpressionOperator.Division => OpType.Divide,
            _ => throw new InvalidOperationException($"Unsupported operator: {op}")
        };
    }
}
