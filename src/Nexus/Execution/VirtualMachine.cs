using System;
using System.Collections.Generic;
using Nexus.Compilation;
using Nexus.Execution.Values;

namespace Nexus.Execution
{
    class VirtualMachine
    {
        public const int SuccessExitCode = 0;
        const int ChunkStartIndex = 0;

        ValueLookupTable _valLookup = new();

        static bool IsTruthy(NexusValue v)
        {
            if (v is NexusBool) 
                return v != new NexusBool(false);
            if (v is NexusNumber) 
                return v != new NexusNumber(0);

            return true;
        }

        public static NexusValue ExecuteTopLevel(Chunk chunk)
        {
            var instance = new VirtualMachine();
            return instance.ExecuteChunk(chunk) ?? new NexusNumber(SuccessExitCode);
        }

        NexusValue? ExecuteChunk(Chunk chunk)
        {
            var chunkIndex = ChunkStartIndex;
            var valStack = new Stack<NexusValue>();
            NexusValue? returnVal = null;

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
                        NexusValue rAdd = valStack.Pop(), lAdd = valStack.Pop();
                        valStack.Push(lAdd + rAdd);
                        break;
                    case InstructionType.Subtract:
                        NexusValue rSub = valStack.Pop(), lSub = valStack.Pop();
                        valStack.Push(lSub - rSub);
                        break;
                    case InstructionType.Multiply:
                        NexusValue rMul = valStack.Pop(), lMul = valStack.Pop();
                        valStack.Push(lMul * rMul);
                        break;
                    case InstructionType.Divide:
                        NexusValue rDiv = valStack.Pop(), lDiv = valStack.Pop();
                        valStack.Push(lDiv / rDiv);
                        break;
                    case InstructionType.EqualTo:
                        NexusValue rEq = valStack.Pop(), lEq = valStack.Pop();
                        valStack.Push(new NexusBool(lEq == rEq));
                        break;
                    case InstructionType.NotEqualTo:
                        NexusValue rNe = valStack.Pop(), lNe = valStack.Pop();
                        valStack.Push(new NexusBool(lNe != rNe));
                        break;
                    case InstructionType.Not:
                        valStack.Push(new NexusBool(!IsTruthy(valStack.Pop())));
                        break;
                    case InstructionType.LessThan:
                        NexusValue rLt = valStack.Pop(), lLt = valStack.Pop();
                        valStack.Push(new NexusBool(lLt < rLt));
                        break;
                    case InstructionType.GreaterThan:
                        NexusValue rGt = valStack.Pop(), lGt = valStack.Pop();
                        valStack.Push(new NexusBool(lGt > rGt));
                        break;
                    case InstructionType.GreaterThanOrEqualTo:
                        NexusValue rGe = valStack.Pop(), lGe = valStack.Pop();
                        valStack.Push(new NexusBool(lGe >= rGe));
                        break;
                    case InstructionType.LessThanOrEqualTo:
                        NexusValue rLe = valStack.Pop(), lLe = valStack.Pop();
                        valStack.Push(new NexusBool(lLe <= rLe));
                        break;
                    case InstructionType.And:
                        NexusValue rAnd = valStack.Pop(), lAnd = valStack.Pop();
                        valStack.Push(new NexusBool(IsTruthy(lAnd) && IsTruthy(rAnd)));
                        break;
                    case InstructionType.Or:
                        NexusValue rOr = valStack.Pop(), lOr = valStack.Pop();
                        valStack.Push(new NexusBool(IsTruthy(lOr) || IsTruthy(rOr)));
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
            returnVal ??= new NexusNumber(SuccessExitCode);
            return returnVal;
        }
    }
}
