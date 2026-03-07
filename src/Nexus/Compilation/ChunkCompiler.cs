using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Execution.Values;
using Nexus.Parsing;
using Nexus.Parsing.Expressions;
using Nexus.Parsing.Statements;

namespace Nexus.Compilation
{
    class ChunkCompiler
    {
        const int FirstVariableId = 0;
        const int ConstantListIndexNotFound = -1;

        public Chunk Chunk => _compiledChunk ?? throw new InvalidOperationException("No chunk has been compiled");

        // Used as an is-dirty flag to avoid accidental reuse of instances       
        readonly BlockNode _blockNode;         
        Chunk? _compiledChunk;
        readonly List<Instruction> _instructions = new();
        readonly List<NexusValue> _constantList = new();

        // Replaces variable names with integers for easier serialization
        readonly Dictionary<string, int> _varNumIdMap = new();

        // The VM will use integers in-place of their identifiers to locate variables from previous stack frames (tables will work differently)
        int _varId = FirstVariableId;

        private ChunkCompiler(BlockNode blockNode, KeyValuePair<string, int>[]? parentVars = null)
        {
            _blockNode = blockNode;

            if (parentVars == null)
                return;
            
            foreach (var pair in parentVars)
                _varNumIdMap[pair.Key] = pair.Value;
        }

        public static Chunk CompileTopLevel(Node root)
        {
            if (root is not SyntaxTree tree)
                throw new ArgumentException($"{nameof(root)} is not a {nameof(SyntaxTree)}");

            if (tree.TopLevelBlockNode is not BlockNode blockNode)
                throw new ArgumentException($"{nameof(tree.TopLevelBlockNode)} is not a {nameof(BlockNode)}");

            var rootCompiler = new ChunkCompiler(blockNode);
            rootCompiler.Compile();
            return rootCompiler.Chunk;
        }

        void Compile()
        {
            if (_compiledChunk is not null)
                throw new InvalidOperationException("A chunk has already been compiled");

            // Indicates that there is a new block that could possibly contain new variable declarations
            _instructions.Add(new Instruction(InstructionType.EnterScope));

            foreach (var node in _blockNode.Statements)
            {
                if (node is ReturnNode returnNode)
                {
                    AddReturnInstructions(returnNode);
                    break; // Any remaining statements will be unreachable, so we ignore them
                }

                if (node is AssignmentNode assignmentNode)
                {
                    AddAssignmentInstructions(assignmentNode);
                    continue;
                }

                throw new ArgumentException($"Unsupported node type: {node.GetType().Name}");
            }

            // After a chunk is executed, all variables declared within the chunk will be removed
            _instructions.Add(new Instruction(InstructionType.ExitScope));
            _compiledChunk = new(_instructions.ToArray(), _constantList.ToArray());
        }

        void AddAssignmentInstructions(AssignmentNode node)
        {
            // Execution: The expression will be evaluated and the result will be pushed to the value stack
            AddExpressionInstructions(node.Expression);

            // Execution: If the variable has not been declared yet, declare it and add it to the variable cache
            if (!_varNumIdMap.TryGetValue(node.Identifier, out var vmCacheIndex))
            {
                vmCacheIndex = _varId++;
                _varNumIdMap[node.Identifier] = vmCacheIndex;
                _instructions.Add(new Instruction(InstructionType.Declare, vmCacheIndex));
            }

            /* Execution: Pop expression result from the value stack, use the variable ID to reference the variable,
             * and assign the result to the variable */
            _instructions.Add(new Instruction(InstructionType.Assign, vmCacheIndex));
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
            if (node is not LiteralNode literal)
            {
                _instructions.Add(new Instruction(InstructionType.PushConstant, ConstantListIndexNotFound));
                return;
            }

            // Convert the token's lexeme to the data type determined by the token type
            var newVal = ParseLiteral(literal);
            var constIndex = ConstantListIndexNotFound;

            // Check for an existing constant that matches the exact value being pushed (NOT A REFERENCE AND NOT COERCED)
            switch (newVal.Type)
            {
                case NexusValueType.Number:
                    constIndex = _constantList.FindIndex(v => v.IsType(NexusValueType.Number) && v.ToDouble() == newVal.ToDouble());
                    break;
                case NexusValueType.Bool:
                    constIndex = _constantList.FindIndex(v => v.IsType(NexusValueType.Bool) && v.ToBool() == newVal.ToBool());
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported constant literal type: {newVal.Type}");
            }

            // If no existing constant was found, add the new value to the constant list and push its index
            if (constIndex == ConstantListIndexNotFound)
            {
                constIndex = _constantList.Count;
                _constantList.Add(newVal);
            }

            _instructions.Add(new Instruction(InstructionType.PushConstant, constIndex));
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
