namespace kasthack.AsyncSynchronized
{
    /// <summary>
    /// Options for creating a synchronized proxy.
    /// </summary>
    /// <param name="BypassPropertyGetterCalls">Do not put locks for property getter calls</param>
    internal record SynchronizedOptions(bool BypassPropertyGetterCalls)
    {
        /// <summary>
        /// Test logger.
        /// </summary>
#if DEBUG
        public Xunit.Abstractions.ITestOutputHelper? Logger { get; init; }
#endif
    }
}