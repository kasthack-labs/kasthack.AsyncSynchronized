namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

public class SynchronizedBenchmark : BenchmarkBase
{
    protected override BenchmarkTarget Target { get; } = new BenchmarkTarget().Synchronized(false);
}
