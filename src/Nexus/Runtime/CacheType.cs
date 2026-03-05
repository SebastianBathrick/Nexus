namespace Nexus.Runtime
{
    /// <summary>
    ///     The type of cache an operation will access when executed by <see cref="Runtime" />.
    /// </summary>
    enum CacheType
    {
        /// <summary> No cache to access. </summary>
        None,

        /// <summary> Cache containing primitive literals. </summary>
        Constant
    }
}
