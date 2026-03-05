namespace Nexus.Runtime
{
    readonly struct Instruction
    {
        const int NoMappedIndex = -1;

        public readonly InstructionType OpType;
        public readonly CacheType CacheType;
        public readonly int CacheIndex;

        public Instruction(InstructionType opType, CacheType cacheType = CacheType.None, int cacheIndex = NoMappedIndex)
        {
            OpType = opType;
            CacheType = cacheType;
            CacheIndex = cacheIndex;
        }

        public override string ToString()
        {
            if (CacheType == CacheType.None)
                return OpType.ToString();

            return $"{OpType}, {CacheType}[{CacheIndex}]";
        }
    }
}
