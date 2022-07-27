namespace kasthack.AsyncSynchronized.Tests
{
    using System.Diagnostics;
    using kasthack.AsyncSynchronized;
    using kasthack.AsyncSynchronized.Tests.Targets;
    using Xunit.Abstractions;

    public record SynchronizedExtensionsTest(ITestOutputHelper Logger)
    {
        [Fact]
        public async Task RawTargetWorks() => await this.AssertTargetProperties(new TestTarget(), 2, 2, 0).ConfigureAwait(false);

        [Fact]
        public async Task InterceptedClassTargetWorks() => await this.AssertTargetProperties(new TestTarget().Synchronized(this.Logger), 1, 1, 0).ConfigureAwait(false);

        public async Task AssertTargetProperties(TestTarget target, int exprectedProgressThreads, int expectedProgressCalls, int expectedProgressReturns)
        {
            target.Logger = this.Logger;

            // initial
            Assert.True(target.ThreadCount == 0, $"pre {nameof(target.ThreadCount)} is {target.ThreadCount} instead of {0} / {target.ID}");
            Assert.True(target.CallCount == 0, $"pre {nameof(target.CallCount)} is {target.CallCount} instead of {0} / {target.ID}");
            Assert.True(target.ReturnCount == 0, $"pre {nameof(target.ReturnCount)} is {target.ReturnCount} instead of {0} / {target.ID}");

            this.Logger.WriteLine($"[{DateTimeOffset.Now:O}]Launching tasks / {target.ID}");

            // immediately after running tasks
            Task t1 = default!, t2 = default!;
            var elapsed1 = Time(() => t1 = target.ThreadUnsafe());
            var elapsed2 = Time(() => t2 = target.ThreadUnsafe());

            // wait for the middle of the operation
            await Task.Delay(TestTarget.OperationTimeInMilliseconds / 2).ConfigureAwait(false);

            this.Logger.WriteLine($"[{DateTimeOffset.Now:O}]Starting fetching of in-progress state / {target.ID}");
            var (actualThreads, actualCalls, actualReturns) = (target.ThreadCount,  target.CallCount, target.ReturnCount);
            this.Logger.WriteLine($"[{DateTimeOffset.Now:O}]Completed fetching of in-progress state / {target.ID}");

            Assert.True(exprectedProgressThreads == actualThreads, $"progress {nameof(target.ThreadCount)} is {actualThreads} instead of {exprectedProgressThreads} / {target.ID}");
            Assert.True(expectedProgressReturns == actualReturns, $"progress {nameof(target.ReturnCount)} is {actualReturns} instead of {expectedProgressReturns} / {target.ID}");
            Assert.True(expectedProgressCalls == actualCalls, $"progress {nameof(target.ReturnCount)} is {actualCalls} instead of {expectedProgressCalls} / {target.ID}");

            foreach (var elapsed in new[] { elapsed1, elapsed2 })
            {
                // https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fdownload.microsoft.com%2Fdownload%2F3%2F0%2F2%2F3027D574-C433-412A-A8B6-5E0A75D5B237%2FTimer-Resolution.docx&wdOrigin=BROWSELINK
                Assert.True(elapsed.TotalMilliseconds <= 30, $"task with immediate return took more than two OS ticks to create / {target.ID}");
                this.Logger.WriteLine($"[{DateTimeOffset.Now:O}]task creation completed in {elapsed}ms / {target.ID}");
            }

            // after completion
            await Task.WhenAll(t1, t2).ConfigureAwait(false);
            Assert.True(target.ThreadCount == 0, $"post {nameof(target.ThreadCount)} is {target.ThreadCount} instead of {0} / {target.ID}");
            Assert.True(target.CallCount == 2, $"post {nameof(target.CallCount)} is {target.CallCount} instead of {2} / {target.ID}");
            Assert.True(target.ReturnCount == 2, $"post {nameof(target.ReturnCount)} is {target.ReturnCount} instead of {2} / {target.ID}");
        }

        private static TimeSpan Time(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            return sw.Elapsed;
        }
    }
}