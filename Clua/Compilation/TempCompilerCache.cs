using Clua.ByteCode;
using Clua.Execution.Values;
using System.Collections.Generic;

namespace Clua.Compilation
{
    class TempCompilerCache
    {
        public readonly List<Op> Instructions;
        public readonly List<CluaValue> Values;

        public TempCompilerCache(List<Op>? instructions = null, List<CluaValue>? values = null)
        {
            Instructions = instructions != null ? instructions : new();
            Values = values != null ? values : new();
        }
    }
}
