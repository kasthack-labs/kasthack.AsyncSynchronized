namespace kasthack.AsyncSynchronized
{

    public static class SynchronizedExtensions
    {
        /// <summary>
        /// Creates an asynchronous synchronized proxy.
        /// </summary>
        /// <typeparam name="T">Type to create proxy for.</typeparam>
        /// <param name="target">Object instance to wrap.</param>
        /// <param name="logger">Test logger</param>
        /// <param name="bypassPropertyGetterCalls">Allow calling property getters without acquiring a lock.</param>
        /// <returns>Synchronized proxy.</returns>
        public static T Synchronized<T>(
            this T target,
#if DEBUG
            Xunit.Abstractions.ITestOutputHelper logger,
#endif
            bool bypassPropertyGetterCalls = true)
            where T : class
            => DynamicProxy.SynchronizedFactory.Create<T>(
                target,
                new SynchronizedOptions(bypassPropertyGetterCalls)
                {
#if DEBUG
                    Logger = logger,
#endif
                });
    }
}