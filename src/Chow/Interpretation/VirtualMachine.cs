using System;
using System.Collections.Generic;
using Chow.Compilation;
using Chow.Values;
namespace Chow.Interpretation
{
    class VirtualMachine
    {
        public const int SuccessExitCode = 0;
        const int ChunkStartIndex = 0;

        static bool IsTruthy(ChowValue v)
        {
            if (v is ChowBool)
                return v != new ChowBool(false);

            if (v is ChowNumber)
                return v != new ChowNumber(0);

            return true;
        }

        public static ChowValue ExecuteTopLevel(Chunk chunk)
        {
            var result = ExecuteChunk(chunk);
            return (object)result != null ? result : new ChowNumber(SuccessExitCode);
        }

        static ChowValue ExecuteChunk(Chunk chunk)
        {
            var chunkIndex = ChunkStartIndex;
            var valStack = new FastStack<ChowValue>();
            var varStack = new VariableStack();
            ChowValue returnVal = null;

            while (chunkIndex < chunk.Length)
            {
                var op = chunk[chunkIndex];

                switch (op.OpType)
                {
                    // [Scope Instructions]================================================================================
                    case InstructionType.EnterScope:
                        break;
                    case InstructionType.ExitScope:
                        var backTrackIndex = chunkIndex - 1;
                        var seenVarIds = new HashSet<int>();

                        while (chunk[backTrackIndex].OpType != InstructionType.EnterScope)
                        {
                            if (backTrackIndex < 0)
                                throw new InvalidOperationException($"No {nameof(InstructionType.EnterScope)} instruction found");

                            var backTrackOp = chunk[backTrackIndex--];
                            if (backTrackOp.OpType == InstructionType.VariableAssignValue && seenVarIds.Add(backTrackOp.CacheId))
                                varStack.Pop();
                        }

                        break;

                    // [Expression Instructions]================================================================================
                    case InstructionType.ConstantPush:
                        valStack.Push(chunk.GetConstant(op.CacheId));
                        break;
                    case InstructionType.Add:
                        ChowValue rAdd = valStack.Pop(), lAdd = valStack.Pop();
                        valStack.Push(lAdd + rAdd);
                        break;
                    case InstructionType.Subtract:
                        ChowValue rSub = valStack.Pop(), lSub = valStack.Pop();
                        valStack.Push(lSub - rSub);
                        break;
                    case InstructionType.Multiply:
                        ChowValue rMul = valStack.Pop(), lMul = valStack.Pop();
                        valStack.Push(lMul * rMul);
                        break;
                    case InstructionType.Divide:
                        ChowValue rDiv = valStack.Pop(), lDiv = valStack.Pop();
                        valStack.Push(lDiv / rDiv);
                        break;
                    case InstructionType.EqualTo:
                        ChowValue rEq = valStack.Pop(), lEq = valStack.Pop();
                        valStack.Push(new ChowBool(lEq == rEq));
                        break;
                    case InstructionType.NotEqualTo:
                        ChowValue rNe = valStack.Pop(), lNe = valStack.Pop();
                        valStack.Push(new ChowBool(lNe != rNe));
                        break;
                    case InstructionType.Not:
                        valStack.Push(new ChowBool(!IsTruthy(valStack.Pop())));
                        break;
                    case InstructionType.LessThan:
                        ChowValue rLt = valStack.Pop(), lLt = valStack.Pop();
                        valStack.Push(new ChowBool(lLt < rLt));
                        break;
                    case InstructionType.GreaterThan:
                        ChowValue rGt = valStack.Pop(), lGt = valStack.Pop();
                        valStack.Push(new ChowBool(lGt > rGt));
                        break;
                    case InstructionType.GreaterThanOrEqualTo:
                        ChowValue rGe = valStack.Pop(), lGe = valStack.Pop();
                        valStack.Push(new ChowBool(lGe >= rGe));
                        break;
                    case InstructionType.LessThanOrEqualTo:
                        ChowValue rLe = valStack.Pop(), lLe = valStack.Pop();
                        valStack.Push(new ChowBool(lLe <= rLe));
                        break;
                    case InstructionType.And:
                        ChowValue rAnd = valStack.Pop(), lAnd = valStack.Pop();
                        valStack.Push(new ChowBool(IsTruthy(lAnd) && IsTruthy(rAnd)));
                        break;
                    case InstructionType.Or:
                        ChowValue rOr = valStack.Pop(), lOr = valStack.Pop();
                        valStack.Push(new ChowBool(IsTruthy(lOr) || IsTruthy(rOr)));
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
            if ((object)returnVal == null) returnVal = new ChowNumber(SuccessExitCode);
            return returnVal;
        }
    }
}
