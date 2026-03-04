namespace Nexus.ByteCode
{
    /// <summary>
    /// The type of cache an operation will access when executed by <see cref="Execution"/>.
    /// </summary>
    enum CacheType
    {
        /// <summary> No cache to access. </summary>
        None,

        /// <summary> Cache containing primitive literals. </summary>
        Constant
    }
}
