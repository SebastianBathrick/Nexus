using System;
using System.Collections.Generic;
using Nexus.Operations;
using Nexus.Compilation;
using Nexus.Runtime.Values;

namespace Nexus.Runtime
{
    static class VirtualMachine
    {
        const int ChunkStartIndex = 0;
        const int SuccessExitCode = 0;

        static bool IsTruthy(NexusValue v)
        {
            if (v is NexusBool) return v != new NexusBool(false);
            if (v is NexusNumber) return v != new NexusNumber(0);
            return true;
        }

        public static NexusValue ExecuteChunk(Chunk chunk)
        {
            var chunkIndex = ChunkStartIndex;
            var valStack = new Stack<NexusValue>();

            while (chunkIndex < chunk.Length)
            {
                var op = chunk[chunkIndex];

                switch (op.OpType)
                {
                    case OpType.PushConstant:
                        valStack.Push(chunk.GetConstant(op.CacheIndex));
                        break;
                    case OpType.Add:
                        NexusValue rAdd = valStack.Pop(), lAdd = valStack.Pop();
                        valStack.Push(lAdd + rAdd);
                        break;
                    case OpType.Subtract:
                        NexusValue rSub = valStack.Pop(), lSub = valStack.Pop();
                        valStack.Push(lSub - rSub);
                        break;
                    case OpType.Multiply:
                        NexusValue rMul = valStack.Pop(), lMul = valStack.Pop();
                        valStack.Push(lMul * rMul);
                        break;
                    case OpType.Divide:
                        NexusValue rDiv = valStack.Pop(), lDiv = valStack.Pop();
                        valStack.Push(lDiv / rDiv);
                        break;
                    case OpType.EqualTo:
                        NexusValue rEq = valStack.Pop(), lEq = valStack.Pop();
                        valStack.Push(new NexusBool(lEq == rEq));
                        break;
                    case OpType.NotEqualTo:
                        NexusValue rNe = valStack.Pop(), lNe = valStack.Pop();
                        valStack.Push(new NexusBool(lNe != rNe));
                        break;
                    case OpType.Not:
                        valStack.Push(new NexusBool(!IsTruthy(valStack.Pop())));
                        break;
                    case OpType.LessThan:
                        NexusValue rLt = valStack.Pop(), lLt = valStack.Pop();
                        valStack.Push(new NexusBool(lLt < rLt));
                        break;
                    case OpType.GreaterThan:
                        NexusValue rGt = valStack.Pop(), lGt = valStack.Pop();
                        valStack.Push(new NexusBool(lGt > rGt));
                        break;
                    case OpType.GreaterThanOrEqualTo:
                        NexusValue rGe = valStack.Pop(), lGe = valStack.Pop();
                        valStack.Push(new NexusBool(lGe >= rGe));
                        break;
                    case OpType.LessThanOrEqualTo:
                        NexusValue rLe = valStack.Pop(), lLe = valStack.Pop();
                        valStack.Push(new NexusBool(lLe <= rLe));
                        break;
                    case OpType.And:
                        NexusValue rAnd = valStack.Pop(), lAnd = valStack.Pop();
                        valStack.Push(new NexusBool(IsTruthy(lAnd) && IsTruthy(rAnd)));
                        break;
                    case OpType.Or:
                        NexusValue rOr = valStack.Pop(), lOr = valStack.Pop();
                        valStack.Push(new NexusBool(IsTruthy(lOr) || IsTruthy(rOr)));
                        break;
                    case OpType.Return:
                        return valStack.Pop();
                    default:
                        throw new InvalidOperationException($"Unknown operation: {op.OpType}");
                }

                chunkIndex++;
            }

            // Temporarily return value at top of stack if it exists, otherwise return 0
            return new NexusNumber(SuccessExitCode);
        }
    }
}
