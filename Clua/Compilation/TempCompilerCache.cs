using Clua.ByteCode;
using Clua.Execution.Values;
namespace Clua.Compilation;

record TempCompilerCache
{
    public List<Op> Instructions { get; } = [];
    public List<CluaValue> Values { get; } = [];
}
