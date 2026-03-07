namespace Chow.Compilation
{
    readonly struct Instruction
    {
        const int NoMappedId = -1;

        public readonly InstructionType OpType;
        public readonly int CacheId;

        public Instruction(InstructionType opType, int cacheId = NoMappedId)
        {
            OpType = opType;
            CacheId = cacheId;
        }

        public override string ToString()
        {
            if (CacheId == NoMappedId)
                return OpType.ToString();

            return $"{OpType}[{CacheId}]";
        }
    }
}
