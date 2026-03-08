using System;
using System.Collections.Generic;
using Chow.Interpretation;
using Chow.Parsing;
using Chow.Parsing.Expressions;
using Chow.Parsing.Statements;
namespace Chow.Compilation
{
    class ChunkCompiler
    {
        const int FirstVariableId = 0;
        const int ConstantListIndexNotFound = -1;

        // Used as an is-dirty flag to avoid accidental reuse of instances
        readonly BlockNode _blockNode;
        readonly List<TaggedUnion> _constantList = new List<TaggedUnion>();
        readonly List<Instruction> _instructions = new List<Instruction>();

        // Replaces variable names with integers for easier serialization
        readonly Dictionary<string, int> _varNumIdMap = new Dictionary<string, int>();
        Chunk _compiledChunk;

        // The VM will use integers in-place of their identifiers to locate variables from previous stack frames (tables will work differently)
        int _varId = FirstVariableId;

        ChunkCompiler(BlockNode blockNode, KeyValuePair<string, int>[] parentVars = null)
        {
            _blockNode = blockNode;

            if (parentVars == null)
                return;

            foreach (var pair in parentVars)
                _varNumIdMap[pair.Key] = pair.Value;
        }

        public Chunk Chunk => _compiledChunk ?? throw new InvalidOperationException("No chunk has been compiled");

        public static Chunk CompileTopLevel(Node root)
        {
            var tree = root as SyntaxTree;
            if (tree == null)
                throw new ArgumentException($"{nameof(root)} is not a {nameof(SyntaxTree)}");

            var blockNode = tree.TopLevelBlockNode as BlockNode;
            if (blockNode == null)
                throw new ArgumentException($"{nameof(tree.TopLevelBlockNode)} is not a {nameof(BlockNode)}");

            var rootCompiler = new ChunkCompiler(blockNode);
            rootCompiler.Compile();
            return rootCompiler.Chunk;
        }

        void Compile()
        {
            if (_compiledChunk != null)
                throw new InvalidOperationException("A chunk has already been compiled");

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

            // After a chunk is executed, all variables declared within the chunk will be removed.
            // CacheId carries the count of variables to pop from the variable stack.
            _instructions.Add(new Instruction(InstructionType.ExitScope, _varId));
            _compiledChunk = new Chunk(_instructions.ToArray(), _constantList.ToArray());
        }

        void AddAssignmentInstructions(AssignmentNode node)
        {
            // Execution: The expression will be evaluated and the result will be pushed to the value stack
            AddExpressionInstructions(node.Expression);

            // Execution: If the variable has not been declared yet, add it to the variable cache
            if (!_varNumIdMap.TryGetValue(node.Identifier, out var vmCacheIndex))
            {
                vmCacheIndex = _varId++;
                _varNumIdMap[node.Identifier] = vmCacheIndex;
            }

            /* Execution: Pop expression result from the value stack, use the variable ID to reference the variable,
             * and assign the result to the variable */
            _instructions.Add(new Instruction(InstructionType.VariableAssignValue, vmCacheIndex));
        }

        void AddReturnInstructions(Node node)
        {
            var returnNode = node as ReturnNode;
            if (returnNode == null)
                throw new ArgumentException("node is not a ReturnNode");

            AddExpressionInstructions(returnNode.Expression);
            _instructions.Add(new Instruction(InstructionType.Return));
        }

        #region List Methods

        TaggedUnion ParseLiteral(LiteralNode node)
        {
            if (node is NumberLiteralNode numNode)
                return new TaggedUnion(numNode.GetNumberValue());
            if (node is BoolLiteralNode boolNode)
                return new TaggedUnion(boolNode.GetBoolValue());

            throw new ArgumentException($"Cannot push constant for node type: {node.GetType().Name}");
        }

        #endregion

        #region Expression Methods

        void AddExpressionInstructions(Node node)
        {
            if (node is UnaryExpressionNode unary)
            {
                AddUnaryExpressionInstructions(unary);
                return;
            }

            var expression = node as ExpressionNode;
            if (expression == null)
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
            if (node is IdentifierNode identifierNode)
            {
                if (!_varNumIdMap.TryGetValue(identifierNode.Identifier, out var varId))
                    throw new InvalidOperationException($"Undefined variable: {identifierNode.Identifier}");
                _instructions.Add(new Instruction(InstructionType.VariablePushValue, varId));
                return;
            }

            var literal = node as LiteralNode;
            if (literal == null)
                throw new InvalidOperationException($"Unsupported node type in push: {node.GetType().Name}");

            // Convert the token's lexeme to the data type determined by the token type
            var newVal = ParseLiteral(literal);
            var constIndex = ConstantListIndexNotFound;

            // Check for an existing constant that matches the exact value being pushed (NOT A REFERENCE AND NOT COERCED)
            switch (newVal.Tag)
            {
                case TagType.Number:
                    constIndex = _constantList.FindIndex(v => v.Tag == TagType.Number && v.NumberValue == newVal.NumberValue);
                    break;
                case TagType.Bool:
                    constIndex = _constantList.FindIndex(v => v.Tag == TagType.Bool && v.BoolValue == newVal.BoolValue);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported constant literal type: {newVal.Tag}");
            }

            // If no existing constant was found, add the new value to the constant list and push its index
            if (constIndex == ConstantListIndexNotFound)
            {
                constIndex = _constantList.Count;
                _constantList.Add(newVal);
            }

            _instructions.Add(new Instruction(InstructionType.ConstantPush, constIndex));
        }

        static InstructionType ToOpType(ExpressionOperator op)
        {
            switch (op)
            {
                case ExpressionOperator.Addition: return InstructionType.Add;
                case ExpressionOperator.Subtraction: return InstructionType.Subtract;
                case ExpressionOperator.Multiplication: return InstructionType.Multiply;
                case ExpressionOperator.Division: return InstructionType.Divide;
                case ExpressionOperator.Inequality: return InstructionType.NotEqualTo;
                case ExpressionOperator.Equality: return InstructionType.EqualTo;
                case ExpressionOperator.GreaterThan: return InstructionType.GreaterThan;
                case ExpressionOperator.GreaterThanOrEqual: return InstructionType.GreaterThanOrEqualTo;
                case ExpressionOperator.LessThan: return InstructionType.LessThan;
                case ExpressionOperator.LessThanOrEqual: return InstructionType.LessThanOrEqualTo;
                case ExpressionOperator.LogicalAnd: return InstructionType.And;
                case ExpressionOperator.LogicalOr: return InstructionType.Or;
                default: throw new InvalidOperationException($"Unsupported operator: {op}");
            }
        }

        #endregion
    }
}
