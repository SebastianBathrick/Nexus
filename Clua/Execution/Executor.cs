using Clua.ByteCode;
using Clua.Compilation;
using Clua.Execution.Values;

namespace Clua.Execution
{
    static class Executor
    {
        const int ChunkStartIndex = 0;
        const int SuccessExitCode = 0;

        public static CluaValue ExecuteChunk(Chunk chunk)
        {
            var chunkIndex = ChunkStartIndex;
            var valStack = new Stack<CluaValue>();

            while (chunkIndex < chunk.Length)
            {
                var op = chunk[chunkIndex];

                switch (op.OpType)
                {
                    case OpType.PushConstant:
                        valStack.Push(chunk.GetConstant(op.CacheIndex));
                        break;
                    case OpType.Add:
                        CluaValue rAdd = valStack.Pop(), lAdd = valStack.Pop();
                        valStack.Push(lAdd + rAdd);
                        break;
                    case OpType.Subtract:
                        CluaValue rSub = valStack.Pop(), lSub = valStack.Pop();
                        valStack.Push(lSub - rSub);
                        break;
                    case OpType.Multiply:
                        CluaValue rMul = valStack.Pop(), lMul = valStack.Pop();
                        valStack.Push(lMul * rMul);
                        break;
                    case OpType.Divide:
                        CluaValue rDiv = valStack.Pop(), lDiv = valStack.Pop();
                        valStack.Push(lDiv / rDiv);
                        break;
                    case OpType.Return:
                        return valStack.Pop();
                    default:
                        throw new InvalidOperationException($"Unknown operation: {op.OpType}");
                }

                chunkIndex++;
            }

            // Temporarily return value at top of stack if it exists, otherwise return 0
            return new CluaNumber(SuccessExitCode);
        }
    }
}
