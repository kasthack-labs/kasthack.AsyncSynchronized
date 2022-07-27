namespace kasthack.AsyncSynchronized.DynamicProxy
{
    using Castle.DynamicProxy;

    /// <summary>
    /// Castle.DynamicProxies-based implementation.
    /// </summary>
    internal static class SynchronizedFactory
    {
        private static readonly ProxyGenerator ProxyGenerator = new();
        private static readonly ProxyGenerationOptions ProxyOptions = new();

        public static T Create<T>(
           this T target,
           SynchronizedOptions options)
            where T : class
        {
            if (target is IProxyTargetAccessor)
            {
                return target;
            }

            var interceptor = new SynchronizedInterceptor(options);
            var result = typeof(T) switch
            {
                { IsInterface: true } => ProxyGenerator.CreateInterfaceProxyWithTarget(target, ProxyOptions, interceptor),
                { IsClass: true } => ProxyGenerator.CreateClassProxyWithTarget(target, ProxyOptions, interceptor),
                _ => throw new ArgumentException($"Couldn't generate proxy for type {typeof(T).Name}", nameof(T)),
            };
            return result;
        }
    }
}