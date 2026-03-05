using System;
using Nexus.Operations;
using Nexus.Runtime.Values;
using Nexus.SyntaxAnalysis;
using Nexus.SyntaxAnalysis.Expressions;
using Nexus.SyntaxAnalysis.Statements;

namespace Nexus.Compilation
{
    static class Compiler
    {
        const int NoConstantIndex = -1;

        public static Chunk CompileFromSyntaxTree(Node root)
        {
            if (root is not SyntaxTree tree)
                throw new ArgumentException($"{nameof(root)} is not a {nameof(SyntaxTree)}");

            if (tree.TopLevelBlockNode is not BlockNode block)
                throw new ArgumentException($"{nameof(tree.TopLevelBlockNode)} is not a {nameof(BlockNode)}");

            // TODO: Add functions, control flow constructs, and more statements.
            var tempData = new TempCompilerCache();
            GetReturnInstructions(block.Statements[0], tempData);
            return new Chunk(tempData.Instructions.ToArray(), tempData.Values.ToArray());
        }

        static void GetReturnInstructions(Node node, TempCompilerCache tempCache)
        {
            if (node is not ReturnNode returnNode)
                throw new ArgumentException("node is not a ReturnNode");

            GetExpressionInstructions(returnNode.Expression, tempCache);
            tempCache.Instructions.Add(new Op(OpType.Return));
        }

        static void GetExpressionInstructions(Node node, TempCompilerCache tempCache)
        {
            if (node is UnaryExpressionNode unary)
            {
                GetExpressionInstructions(unary.Operand, tempCache);
                if (unary.Operator == ExpressionOperator.LogicalNot)
                    tempCache.Instructions.Add(new Op(OpType.Not));
                else
                    throw new InvalidOperationException($"Unsupported unary operator: {unary.Operator}");
                return;
            }

            if (node is not ExpressionNode expression)
            {
                GetPushInstruction(node, tempCache);
                return;
            }

            GetExpressionInstructions(expression.Left, tempCache);
            GetExpressionInstructions(expression.Right, tempCache);
            tempCache.Instructions.Add(new Op(GetOpType(expression.Operator)));
        }

        static NexusValue NodeToConstant(Node node)
        {
            if (node is NumberLiteralNode num) return new NexusNumber(num.GetNumberValue());
            if (node is BoolLiteralNode b) return new NexusBool(b.GetBoolValue());
            throw new ArgumentException($"Cannot push constant for node type: {node.GetType().Name}");
        }

        static void GetPushInstruction(Node node, TempCompilerCache tempCache)
        {
            var value = NodeToConstant(node);
            var constIndex = tempCache.Values.FindIndex(x => x == value);

            if (constIndex == NoConstantIndex)
            {
                tempCache.Values.Add(value);
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
                ExpressionOperator.Inequality => OpType.NotEqualTo,
                ExpressionOperator.Equality => OpType.EqualTo,
                ExpressionOperator.GreaterThan => OpType.GreaterThan,
                ExpressionOperator.GreaterThanOrEqual => OpType.GreaterThanOrEqualTo,
                ExpressionOperator.LessThan => OpType.LessThan,
                ExpressionOperator.LessThanOrEqual => OpType.LessThanOrEqualTo,
                ExpressionOperator.LogicalAnd => OpType.And,
                ExpressionOperator.LogicalOr => OpType.Or,
                _ => throw new InvalidOperationException($"Unsupported operator: {op}")
            };
        }
    }
}
