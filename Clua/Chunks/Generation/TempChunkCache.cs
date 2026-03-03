using Clua.Execution.Values;
namespace Clua.Chunks.Generation;

record TempChunkCache
{
    public List<Op> Instructions { get; } = [];
    public List<CluaValue> Values { get; } = [];
}
