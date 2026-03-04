namespace Clua.ByteCode;

readonly struct Op(OpType opType, CacheType cacheType = CacheType.None, int cacheIndex = Op.NoMappedIndex)
{
    const int NoMappedIndex = -1;

    public OpType OpType { get; } = opType;
    public CacheType CacheType { get; } = cacheType;
    public int CacheIndex { get; } = cacheIndex;

    public override string ToString()
    {
        if (CacheType == CacheType.None)
            return OpType.ToString();

        return $"{OpType}, {CacheType}[{CacheIndex}]";
    }
}
