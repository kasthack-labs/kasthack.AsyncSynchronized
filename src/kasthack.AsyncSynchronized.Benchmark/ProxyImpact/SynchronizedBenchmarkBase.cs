namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public abstract class BenchmarkBase
{
    protected abstract BenchmarkTarget Target { get; }

    [Benchmark]
    public Task AsyncTask() => this.Target.AsyncTask();

    [Benchmark]
    public Task<int> AsyncTaskWithResult() => this.Target.AsyncTaskWithResult();

    [Benchmark]
    public int Int() => this.Target.Int();

    [Benchmark]
    public int Property() => _ = this.Target.Property;

    [Benchmark]
    public Task<int> TaskWithResult() => this.Target.TaskWithResult();

    [Benchmark]
    public void Void() => this.Target.Void();
}
