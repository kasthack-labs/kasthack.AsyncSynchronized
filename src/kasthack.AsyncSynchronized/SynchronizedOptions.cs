namespace kasthack.AsyncSynchronized
{
    internal record SynchronizedOptions(bool AllowGetters)
    {
#if DEBUG
        public Xunit.Abstractions.ITestOutputHelper? Logger { get; init; }
#endif
    }
}