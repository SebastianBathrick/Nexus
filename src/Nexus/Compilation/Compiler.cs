using System;
using System.Collections.Generic;
using Nexus.Runtime;
using Nexus.Runtime.Values;
using Nexus.SyntaxAnalysis;
using Nexus.SyntaxAnalysis.Expressions;
using Nexus.SyntaxAnalysis.Statements;
using Nexus.SyntaxAnalysis.Expressions.Literals;

namespace Nexus.Compilation
{
    class Compiler
    {
        const int NoConstantIndex = -1;

        List<Instruction> _instructions = new List<Instruction>();
        List<NexusValue> _constantList = new List<NexusValue>();

        public Chunk CompileFromSyntaxTree(Node root)
        {
            if (root is not SyntaxTree tree)
                throw new ArgumentException($"{nameof(root)} is not a {nameof(SyntaxTree)}");

            if (tree.TopLevelBlockNode is not BlockNode blockNode)
                throw new ArgumentException($"{nameof(tree.TopLevelBlockNode)} is not a {nameof(BlockNode)}");

            return CreateChunk(blockNode);
        }

        Chunk CreateChunk(BlockNode blockNode)
        {
            // Reset the state from previous compilation
            _instructions = new();
            _constantList = new();

            foreach (var node in blockNode.Statements)
            {
                if (node is ReturnNode returnNode)
                    ToReturnInstructions(returnNode);
                else if (node is AssignmentNode assignmentNode)
                    ToAssignmentInstructions(assignmentNode);
                else
                    throw new ArgumentException($"Unsupported node type: {node.GetType().Name}");
            }

            return new(_instructions.ToArray(), _constantList.ToArray());
        }

        void ToAssignmentInstructions(AssignmentNode node)
        {
            ToExpressionInstructions(node.Expression);
            _instructions.Add(new Instruction(InstructionType.Assign));
        }

        void ToReturnInstructions(Node node)
        {
            if (node is not ReturnNode returnNode)
                throw new ArgumentException("node is not a ReturnNode");

            ToExpressionInstructions(returnNode.Expression);
            _instructions.Add(new Instruction(InstructionType.Return));
        }

        void ToPushInstruction(Node node)
        {
            if (node is LiteralNode)
            {
                var constantIndex = _constantList.Count;
                var cacheType = CacheType.Constant;
                AddToConstantList(node);

                _instructions.Add(new(InstructionType.PushConstant, cacheType, constantIndex));
            }
        }


        #region Expression Methods

        void ToExpressionInstructions(Node node)
        {
            if (node is UnaryExpressionNode unary)
            {
                ToUnaryExpressionInstructions(unary);
                return;
            }

            if (node is not ExpressionNode expression)
            {
                ToPushInstruction(node);
                return;
            }

            ToExpressionInstructions(expression.Left);
            ToExpressionInstructions(expression.Right);
            _instructions.Add(new Instruction(ToOpType(expression.Operator)));
        }

        void ToUnaryExpressionInstructions(UnaryExpressionNode node)
        {
            ToExpressionInstructions(node.Operand);
            if (node.Operator == ExpressionOperator.LogicalNot)
                _instructions.Add(new Instruction(InstructionType.Not));
            else
                throw new InvalidOperationException($"Unsupported unary operator: {node.Operator}");
        }

        static InstructionType ToOpType(ExpressionOperator op)
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

        #endregion

        #region List Methods

        NexusValue AddToConstantList(Node node)
        {
            NexusValue val;

            if (node is NumberLiteralNode numNode)
                val = new NexusNumber(numNode.GetNumberValue());
            else if (node is BoolLiteralNode boolNode)
                val = new NexusBool(boolNode.GetBoolValue());
            else
                throw new ArgumentException($"Cannot push constant for node type: {node.GetType().Name}");

            _constantList.Add(val);
            return val;
        }

        #endregion
    }
}
