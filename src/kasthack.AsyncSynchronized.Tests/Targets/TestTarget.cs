namespace kasthack.AsyncSynchronized.Tests.Targets
{
    using Xunit.Abstractions;

    public class TestTarget : ITestTarget
    {
        public const int OperationTimeInMilliseconds = 1000;

        private static int counter = 0;
        private int threadCount;
        private int returnCount;
        private int callCount;

        // ID for debugging purposes
        public virtual int ID { get; } = Interlocked.Increment(ref counter);

        public virtual ITestOutputHelper? Logger { get; set; }

        public virtual int CallCount => this.callCount;

        public virtual int ReturnCount => this.returnCount;

        public virtual int ThreadCount => this.threadCount;

        public virtual async Task ThreadUnsafe()
        {
            var callCount = Interlocked.Increment(ref this.callCount);
            this.Logger?.WriteLine($"[{DateTimeOffset.Now:O}]Incremeneted {nameof(this.CallCount)} to {callCount} / {this.ID}");

            var progressThreadCount = Interlocked.Increment(ref this.threadCount);
            this.Logger?.WriteLine($"[{DateTimeOffset.Now:O}]Incremeneted {nameof(this.ThreadCount)} to {progressThreadCount} / {this.ID}");

            await Task.Delay(OperationTimeInMilliseconds).ConfigureAwait(false);

            var completedThreadCount = Interlocked.Decrement(ref this.threadCount);
            this.Logger?.WriteLine($"[{DateTimeOffset.Now:O}]Decremeneted {nameof(this.ThreadCount)} to {completedThreadCount} / {this.ID}");

            var returnCount = Interlocked.Increment(ref this.returnCount);
            this.Logger?.WriteLine($"[{DateTimeOffset.Now:O}]Incremeneted {nameof(this.ReturnCount)} to {returnCount} / {this.ID}");
        }
    }
}