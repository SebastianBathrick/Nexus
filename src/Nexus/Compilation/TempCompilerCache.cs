using System.Collections.Generic;
using Nexus.ByteCode;
using Nexus.Execution.Values;

namespace Nexus.Compilation
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
