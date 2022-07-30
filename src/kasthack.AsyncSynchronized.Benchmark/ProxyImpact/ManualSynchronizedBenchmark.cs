namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

using kasthack.AsyncSynchronized.Benchmark.Targets;

public class ManualSynchronizedBenchmark : BenchmarkBase
{
    protected override BenchmarkTarget Target { get; } = new SyncBenchmarkTarget();
}
