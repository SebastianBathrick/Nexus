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

        Dictionary<string, int> _varIndexMap = new Dictionary<string, int>();
        List<string> _currChunkVarNames = new List<string>();
        int _nextVarCacheIndex = 0;
        
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
                    AddReturnInstructions(returnNode);
                else if (node is AssignmentNode assignmentNode)
                    AddAssignmentInstructions(assignmentNode);
                else
                    throw new ArgumentException($"Unsupported node type: {node.GetType().Name}");
            }

            // Removes variables to avoid name conflicts with declarations made after this chunk (e.g. control structure or function blocks)
            foreach (var varName in _currChunkVarNames)
                _varIndexMap.Remove(varName);

            return new(_instructions.ToArray(), _constantList.ToArray());
        }

        void AddAssignmentInstructions(AssignmentNode node)
        {
            // Execution: The expression will be evaluated and the result will be pushed to the value stack
            AddExpressionInstructions(node.Expression);

            // Execution: If the variable has not been declared yet, declare it and add it to the variable cache
            if (!_varIndexMap.TryGetValue(node.Identifier, out var vmCacheIndex))
            {
                vmCacheIndex = _nextVarCacheIndex++;
                _varIndexMap[node.Identifier] = vmCacheIndex;

                // Add to use as a key for _varIndexMap when removing this chunk's variables
                _currChunkVarNames.Add(node.Identifier);
                _instructions.Add(new Instruction(InstructionType.Declare, CacheType.Variable, vmCacheIndex));
            }

            /* Execution: Pop expression result from the value stack, use the variable cache index to reference the variable,
             * and assign the expression result to the variable */
            _instructions.Add(new Instruction(InstructionType.Assign, CacheType.Variable, vmCacheIndex));
        }

        void AddReturnInstructions(Node node)
        {
            if (node is not ReturnNode returnNode)
                throw new ArgumentException("node is not a ReturnNode");

            

            AddExpressionInstructions(returnNode.Expression);
            _instructions.Add(new Instruction(InstructionType.Return));
        }




        #region Expression Methods

        void AddExpressionInstructions(Node node)
        {
            if (node is UnaryExpressionNode unary)
            {
                AddUnaryExpressionInstructions(unary);
                return;
            }

            if (node is not ExpressionNode expression)
            {
                AddPushInstruction(node);
                return;
            }

            AddExpressionInstructions(expression.Left);
            AddExpressionInstructions(expression.Right);
            _instructions.Add(new Instruction(ToOpType(expression.Operator)));
        }

        void AddUnaryExpressionInstructions(UnaryExpressionNode node)
        {
            AddExpressionInstructions(node.Operand);
            if (node.Operator == ExpressionOperator.LogicalNot)
                _instructions.Add(new Instruction(InstructionType.Not));
            else
                throw new InvalidOperationException($"Unsupported unary operator: {node.Operator}");
        }

        void AddPushInstruction(Node node)
        {
            if (node is LiteralNode literal)
            {
                var value = ParseLiteral(literal);
                _constantList.Add(value);
                _instructions.Add(new Instruction(InstructionType.PushConstant, CacheType.Constant,
                    _constantList.Count - 1));
            }
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

        NexusValue ParseLiteral(LiteralNode node)
        {
            NexusValue val;

            if (node is NumberLiteralNode numNode)
                val = new NexusNumber(numNode.GetNumberValue());
            else if (node is BoolLiteralNode boolNode)
                val = new NexusBool(boolNode.GetBoolValue());
            else
                throw new ArgumentException($"Cannot push constant for node type: {node.GetType().Name}");

            return val;
        }

        #endregion
    }
}
