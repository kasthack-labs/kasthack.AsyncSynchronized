namespace kasthack.AsyncSynchronized.DynamicProxy
{
    using System.Threading.Tasks;
    using Castle.DynamicProxy;
    using kasthack.AsyncSynchronized;

    internal class SynchronizedInterceptor : AsyncInterceptorBase, IDisposable
    {
        private readonly SynchronizedOptions options;
        private bool disposed;
        private SemaphoreSlim accessSemaphore = new(1);

        public SynchronizedInterceptor(SynchronizedOptions options)
        {
            this.options = options;
        }

        ~SynchronizedInterceptor()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(SynchronizedInterceptor));
            }

            var targetMethod = invocation.Method;
            if (this.options.AllowGetters)
            {
                // https://stackoverflow.com/a/34506781/17594255
                var isGetter = targetMethod.IsSpecialName && targetMethod.ReturnType != typeof(void) && targetMethod.GetParameters().Length == 0;
                if (isGetter)
                {
#if DEBUG
                    this.options.Logger?.WriteLine($"[{DateTimeOffset.Now:O}]Unrestricted call to {targetMethod.Name}");
#endif
                    await proceed(invocation, proceedInfo).ConfigureAwait(false);
                    return;
                }
            }

            try
            {
#if DEBUG
                this.options.Logger?.WriteLine($"[{DateTimeOffset.Now:O}]Synchronized call to {targetMethod.Name}");
#endif
                await this.accessSemaphore.WaitAsync().ConfigureAwait(false);
                await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }
            finally
            {
                this.accessSemaphore.Release();
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            TResult result = default!;
            await this.InterceptAsync(
                invocation,
                proceedInfo,
                async (i, ipi) =>
                {
                    // reuse non-generic interceptor
                    result = await proceed(i, ipi).ConfigureAwait(false);
                    return; // return is required for picking the correct overload
                }).ConfigureAwait(false);
            return result;
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                this.accessSemaphore?.Dispose();
            }

            this.accessSemaphore = null!;
        }
    }
}