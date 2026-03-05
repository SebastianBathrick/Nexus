namespace Nexus.Runtime
{
    /// <summary>
    ///     The type of cache an operation will access when executed by <see cref="Runtime" />.
    /// </summary>
    enum CacheType : uint
    {
        /// <summary> No cache to access. </summary>
        None = 0,

        /// <summary> Cache containing primitive literals. </summary>
        Constant = 1,

        /// <summary> Cache containing variables. </summary>
        Variable
    }
}
