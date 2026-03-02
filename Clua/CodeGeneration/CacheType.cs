namespace Clua.CodeGeneration;

/// <summary>
/// The type of cache an instruction will access when executed by <see cref="VirtualMachine"/>.
/// </summary>
public enum CacheType
{
    /// <summary> No cache to access. </summary>
    None,
    
    /// <summary> Cache containing primitive literals. </summary>
    Constant
}
