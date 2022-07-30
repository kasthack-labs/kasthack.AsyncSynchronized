namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

using kasthack.AsyncSynchronized.Benchmark.Targets;

public class SynchronizedBenchmark : BenchmarkBase
{
    protected override BenchmarkTarget Target { get; } = new BenchmarkTarget().Synchronized(false);
}
