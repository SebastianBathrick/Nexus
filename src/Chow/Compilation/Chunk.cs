using System.Text;
using Chow.Execution.Values;

namespace Chow.Compilation
{
    class Chunk
    {
        readonly ChowValue[] _constsCache;
        readonly Instruction[] _instructions;

        public Chunk(Instruction[] instructions, ChowValue[] constsCache)
        {
            _instructions = instructions;
            _constsCache = constsCache;
        }

        public int Length => _instructions.Length;

        public Instruction this[int index] => _instructions[index];

        public ChowValue GetConstant(int index) => _constsCache[index];

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("\tCONSTANTS:");

            for (var i = 0; i < _constsCache.Length; i++)
                sb.AppendLine($"\t\t[{i}] {_constsCache[i]}");

            sb.AppendLine("\tINSTRUCTIONS:");

            for (var i = 0; i < _instructions.Length; i++)
                sb.AppendLine($"\t\t[{i}] {_instructions[i]}");

            return sb.ToString();
        }
    }
}
