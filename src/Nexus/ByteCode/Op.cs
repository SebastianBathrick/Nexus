namespace Nexus.ByteCode
{
    readonly struct Op
    {
        const int NoMappedIndex = -1;

        public readonly OpType OpType;
        public readonly CacheType CacheType;
        public readonly int CacheIndex;

        public Op(OpType opType, CacheType cacheType = CacheType.None, int cacheIndex = NoMappedIndex)
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
