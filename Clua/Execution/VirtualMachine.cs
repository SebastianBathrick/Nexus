using Clua.Utilities;
using Clua.Values;
namespace Clua.CodeGeneration;

static class VirtualMachine
{
    const int ChunkStartIndex = 0;

    public static CluaValue ExecuteCode(CodeObject codeObject)
    {
        var chunkIndex = ChunkStartIndex;
        var valStack = new FastStack<CluaValue>();

        while (chunkIndex < codeObject.Length)
        {
            var inst = codeObject[chunkIndex];

            switch (inst.Operation)
            {
                case Operation.PushConstant:
                    valStack.Push(codeObject.GetConstant(inst.CacheIndex));
                    break;
                case Operation.Add:
                    CluaValue rAdd = valStack.Pop(), lAdd = valStack.Pop();
                    valStack.Push(lAdd + rAdd);
                    break;
                case Operation.Subtract:
                    CluaValue rSub = valStack.Pop(), lSub = valStack.Pop();
                    valStack.Push(lSub - rSub);
                    break;
                case Operation.Multiply:
                    CluaValue rMul = valStack.Pop(), lMul = valStack.Pop();
                    valStack.Push(lMul * rMul);
                    break;
                case Operation.Divide:
                    CluaValue rDiv = valStack.Pop(), lDiv = valStack.Pop();
                    valStack.Push(lDiv / rDiv);
                    break;
                case Operation.Return:
                    return valStack.Pop();
                default:
                    throw new InvalidOperationException($"Unknown operation: {inst.Operation}");
            }

            chunkIndex++;
        }

        // Temporarily return value at top of stack if it exists, otherwise return 0
        return valStack.Any() ? valStack.Pop() : new CluaNumber(0);
    }
}
