using System.Collections.Generic;
using Nexus.Operations;
using Nexus.Execution.Values;

namespace Nexus.Compilation
{
    class TempCompilerCache
    {
        public readonly List<Op> Instructions;
        public readonly List<NexusValue> Values;

        public TempCompilerCache(List<Op>? instructions = null, List<NexusValue>? values = null)
        {
            Instructions = instructions != null ? instructions : new();
            Values = values != null ? values : new();
        }
    }
}
