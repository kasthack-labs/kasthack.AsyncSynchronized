namespace kasthack.AsyncSynchronized
{
    using kasthack.AsyncSynchronized.DynamicProxy;

    public static class SynchronizedExtensions
    {
        public static T Synchronized<T>(
            this T target,
#if DEBUG
            Xunit.Abstractions.ITestOutputHelper logger,
#endif
            bool allowGetters = true)
            where T : class
            => SynchronizedFactory.Create<T>(
                target,
                new SynchronizedOptions(allowGetters)
                {
#if DEBUG
                    Logger = logger,
#endif
                });
    }
}