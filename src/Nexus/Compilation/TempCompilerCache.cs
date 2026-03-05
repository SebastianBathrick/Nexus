using System.Collections.Generic;
using Nexus.Runtime;
using Nexus.Runtime.Values;
namespace Nexus.Compilation
{
    class TempCompilerCache
    {
        public readonly List<Instruction> Instructions;
        public readonly List<NexusValue> Values;

        public TempCompilerCache(List<Instruction>? instructions = null, List<NexusValue>? values = null)
        {
            Instructions = instructions != null ? instructions : new List<Instruction>();
            Values = values != null ? values : new List<NexusValue>();
        }
    }
}
