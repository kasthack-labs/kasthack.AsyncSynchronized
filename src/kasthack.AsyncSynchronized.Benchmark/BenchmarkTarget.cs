namespace kasthack.AsyncSynchronized.Benchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class BenchmarkTarget
{
    public virtual Task<int> TaskWithResult() => Task.FromResult(1);

    public virtual async Task<int> AsyncTaskWithResult() => 1;

    public virtual async Task AsyncTask() { }

    public virtual void Void() { }

    public virtual int Int() => 1;

    public virtual int Property => 1;
}