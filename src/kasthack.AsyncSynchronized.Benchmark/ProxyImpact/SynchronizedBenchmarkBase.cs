namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using kasthack.AsyncSynchronized.Benchmark.Targets;

public abstract class BenchmarkBase
{
    private const int Loops = 10_000;

    protected abstract BenchmarkTarget Target { get; }

    [Benchmark]
    public Task AsyncTask() => this.Target.AsyncTask();

    [Benchmark]
    public Task<int> AsyncTaskWithResult() => this.Target.AsyncTaskWithResult();

    [Benchmark]
    public int Int() => this.Target.Int();

    [Benchmark]
    public int Property() => _ = this.Target.Property;

    [Benchmark(OperationsPerInvoke = Loops)]
    public async Task<int> TaskWithResult()
    {
        var result = 0;
        for (int i = 0; i < Loops; i++)
        {
            result += await this.Target.TaskWithResult().ConfigureAwait(false);
        }

        return result;
    }

    [Benchmark]
    public void Void() => this.Target.Void();
}
