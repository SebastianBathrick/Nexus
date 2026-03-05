using System;
using Nexus.Runtime;
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
            tempCache.Instructions.Add(new Instruction(InstructionType.Return));
        }

        static void GetExpressionInstructions(Node node, TempCompilerCache tempCache)
        {
            if (node is UnaryExpressionNode unary)
            {
                GetExpressionInstructions(unary.Operand, tempCache);
                if (unary.Operator == ExpressionOperator.LogicalNot)
                    tempCache.Instructions.Add(new Instruction(InstructionType.Not));
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
            tempCache.Instructions.Add(new Instruction(GetOpType(expression.Operator)));
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

            tempCache.Instructions.Add(new Instruction(InstructionType.PushConstant, CacheType.Constant, constIndex));
        }

        static InstructionType GetOpType(ExpressionOperator op)
        {
            return op switch
            {
                ExpressionOperator.Addition => InstructionType.Add,
                ExpressionOperator.Subtraction => InstructionType.Subtract,
                ExpressionOperator.Multiplication => InstructionType.Multiply,
                ExpressionOperator.Division => InstructionType.Divide,
                ExpressionOperator.Inequality => InstructionType.NotEqualTo,
                ExpressionOperator.Equality => InstructionType.EqualTo,
                ExpressionOperator.GreaterThan => InstructionType.GreaterThan,
                ExpressionOperator.GreaterThanOrEqual => InstructionType.GreaterThanOrEqualTo,
                ExpressionOperator.LessThan => InstructionType.LessThan,
                ExpressionOperator.LessThanOrEqual => InstructionType.LessThanOrEqualTo,
                ExpressionOperator.LogicalAnd => InstructionType.And,
                ExpressionOperator.LogicalOr => InstructionType.Or,
                _ => throw new InvalidOperationException($"Unsupported operator: {op}")
            };
        }
    }
}
