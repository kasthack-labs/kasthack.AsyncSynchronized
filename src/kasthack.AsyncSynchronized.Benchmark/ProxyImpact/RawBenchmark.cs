namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using kasthack.AsyncSynchronized.Benchmark.Targets;

public class RawBenchmark : BenchmarkBase
{
    protected override BenchmarkTarget Target { get; } = new BenchmarkTarget();
}
