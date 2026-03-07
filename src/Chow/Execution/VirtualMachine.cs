using System;
using System.Collections.Generic;
using Chow.Compilation;
using Chow.Execution.Values;

namespace Chow.Execution
{
    class VirtualMachine
    {
        public const int SuccessExitCode = 0;
        const int ChunkStartIndex = 0;

        ValueLookupTable _valLookup = new();

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
            var instance = new VirtualMachine();
            return instance.ExecuteChunk(chunk) ?? new ChowNumber(SuccessExitCode);
        }

        ChowValue? ExecuteChunk(Chunk chunk)
        {
            var chunkIndex = ChunkStartIndex;
            var valStack = new Stack<ChowValue>();
            ChowValue? returnVal = null;

            while (chunkIndex < chunk.Length)
            {
                var op = chunk[chunkIndex];

                switch (op.OpType)
                {
                    // [Scope Instructions]================================================================================
                    case InstructionType.EnterScope:
                        _valLookup.EnterScope();
                        break;
                    case InstructionType.ExitScope:
                        // This case is if the scope executed all instructions without returning
                        _valLookup.ExitScope();

                        // If null chunk represents a control structure block or void function
                        return returnVal;

                    // [Expression Instructions]================================================================================
                    case InstructionType.PushConstant:
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
                    case InstructionType.Declare:
                        _valLookup.AddIdentifier(op.CacheId);
                        break;
                    case InstructionType.Assign:
                        _valLookup.SetValue(op.CacheId, valStack.Pop());
                        break;
                     
                    default:
                        throw new InvalidOperationException($"Unknown operation: {op.OpType}");
                }

                chunkIndex++;
            }

            // If no exit code was returned on the top-level then return the success exit code
            returnVal ??= new ChowNumber(SuccessExitCode);
            return returnVal;
        }
    }
}
