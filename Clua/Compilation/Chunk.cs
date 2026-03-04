using System.Text;
using Clua.ByteCode;
using Clua.Execution.Values;

namespace Clua.Compilation
{
    class Chunk
    {
        readonly Op[] _instructions;
        readonly CluaValue[] _constsCache;

        public int Length => _instructions.Length;

        public Chunk(Op[] instructions, CluaValue[] constsCache)
        {
            _instructions = instructions;
            _constsCache = constsCache;
        }

        public Op this[int index] => _instructions[index];

        public CluaValue GetConstant(int index) => _constsCache[index];

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("[CONSTANTS]:\n");

            for (var i = 0; i < _constsCache.Length; i++)
                sb.AppendLine($"[{i}] {_constsCache[i]}");

            sb.AppendLine("\n[INSTRUCTIONS]:\n");

            for (var i = 0; i < _instructions.Length; i++)
                sb.AppendLine($"[{i}] {_instructions[i]}");

            return sb.ToString();
        }
    }
}
