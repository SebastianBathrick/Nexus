using System.Text;
using Clua.Execution.Values;
namespace Clua.Chunks;

class Chunk(Op[] instructions, CluaValue[] constsCache)
{
    public int Length => instructions.Length;

    public Op this[int index] => instructions[index];

    public CluaValue GetConstant(int index) => constsCache[index];

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("[CONSTANTS]:\n");

        for (var i = 0; i < constsCache.Length; i++)
            sb.AppendLine($"[{i}] {constsCache[i]}");

        sb.AppendLine("\n[INSTRUCTIONS]:\n");

        for(var i = 0; i < instructions.Length; i++)
            sb.AppendLine($"[{i}] {instructions[i]}");

        return sb.ToString();
    }
}
