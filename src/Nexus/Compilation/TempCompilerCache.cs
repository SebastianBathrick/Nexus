using System.Collections.Generic;
using Nexus.Operations;
using Nexus.Runtime.Values;

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
