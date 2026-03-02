namespace Clua.CodeGeneration;

readonly struct Instruction(Operation operation, CacheType cacheType = CacheType.None, int cacheIndex = Instruction.NoMappedIndex)
{
    const int NoMappedIndex = -1;

    public Operation Operation { get; } = operation;
    public CacheType CacheType { get; } = cacheType;
    public int CacheIndex { get; } = cacheIndex;

    public override string ToString()
    {
        if (CacheType == CacheType.None)
            return Operation.ToString();
        
        return $"{Operation}, {CacheType}[{CacheIndex}]";
    }
}