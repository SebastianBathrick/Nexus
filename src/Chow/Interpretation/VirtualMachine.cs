using System;
using Chow.Compilation;
namespace Chow.Interpretation
{
    class VirtualMachine
    {
        public const int SuccessExitCode = 0;
        const int ChunkStartIndex = 0;

        public static TaggedUnion ExecuteTopLevel(Chunk chunk)
        {
            return ExecuteChunk(chunk);
        }

        static TaggedUnion ExecuteChunk(Chunk chunk)
        {
            var chunkIndex = ChunkStartIndex;
            var valStack = new FastStack<TaggedUnion>();
            var varStack = new VariableStack();
            TaggedUnion? returnVal = null;

            while (chunkIndex < chunk.Length)
            {
                var op = chunk[chunkIndex];

                switch (op.OpType)
                {
                    // [Scope Instructions]================================================================================
                    case InstructionType.EnterScope:
                        break;
                    case InstructionType.ExitScope:
                        for (var i = 0; i < op.CacheId; i++)
                            varStack.Pop();
                        break;

                    // [Expression Instructions]================================================================================
                    case InstructionType.ConstantPush:
                        valStack.Push(chunk.GetConstant(op.CacheId));
                        break;
                    case InstructionType.Add:
                        var rAdd = valStack.Pop();
                        var lAdd = valStack.Pop();
                        valStack.Push(lAdd + rAdd);
                        break;
                    case InstructionType.Subtract:
                        var rSub = valStack.Pop();
                        var lSub = valStack.Pop();
                        valStack.Push(lSub - rSub);
                        break;
                    case InstructionType.Multiply:
                        var rMul = valStack.Pop();
                        var lMul = valStack.Pop();
                        valStack.Push(lMul * rMul);
                        break;
                    case InstructionType.Divide:
                        var rDiv = valStack.Pop();
                        var lDiv = valStack.Pop();
                        valStack.Push(lDiv / rDiv);
                        break;
                    case InstructionType.EqualTo:
                        var rEq = valStack.Pop();
                        var lEq = valStack.Pop();
                        valStack.Push(lEq == rEq);
                        break;
                    case InstructionType.NotEqualTo:
                        var rNe = valStack.Pop();
                        var lNe = valStack.Pop();
                        valStack.Push(lNe != rNe);
                        break;
                    case InstructionType.Not:
                        valStack.Push(TaggedUnion.Not(valStack.Pop()));
                        break;
                    case InstructionType.LessThan:
                        var rLt = valStack.Pop();
                        var lLt = valStack.Pop();
                        valStack.Push(lLt < rLt);
                        break;
                    case InstructionType.GreaterThan:
                        var rGt = valStack.Pop();
                        var lGt = valStack.Pop();
                        valStack.Push(lGt > rGt);
                        break;
                    case InstructionType.GreaterThanOrEqualTo:
                        var rGe = valStack.Pop();
                        var lGe = valStack.Pop();
                        valStack.Push(lGe >= rGe);
                        break;
                    case InstructionType.LessThanOrEqualTo:
                        var rLe = valStack.Pop();
                        var lLe = valStack.Pop();
                        valStack.Push(lLe <= rLe);
                        break;
                    case InstructionType.And:
                        var rAnd = valStack.Pop();
                        var lAnd = valStack.Pop();
                        valStack.Push(TaggedUnion.And(lAnd, rAnd));
                        break;
                    case InstructionType.Or:
                        var rOr = valStack.Pop();
                        var lOr = valStack.Pop();
                        valStack.Push(TaggedUnion.Or(lOr, rOr));
                        break;

                    // [Statement Instructions]================================================================================
                    case InstructionType.Return:
                        // Store it, because the following instruction is ExitScope which will return the value
                        returnVal = valStack.Pop();
                        break;
                    // TODO: Declaration parsing is no longer required
                    case InstructionType.VariableAssignValue:
                        var targetVar = varStack.LookUp(op.CacheId);
                        var assignVal = valStack.Pop();

                        if (targetVar == null)
                            varStack.Push(new Variable(op.CacheId, assignVal));
                        else
                            targetVar.Assign(assignVal);
                        break;

                    case InstructionType.VariablePushValue:
                        var valueVar = varStack.LookUp(op.CacheId);
                        if (valueVar == null)
                            throw new InvalidOperationException($"Variable {op.CacheId} not found");
                        valStack.Push(valueVar.Value);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown operation: {op.OpType}");
                }

                chunkIndex++;
            }

            // If no exit code was returned on the top-level then return the success exit code
            if (!returnVal.HasValue) returnVal = new TaggedUnion(SuccessExitCode);
            return returnVal.Value;
        }
    }
}
