namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class RawBenchmark : BenchmarkBase
{
    protected override BenchmarkTarget Target { get; } = new BenchmarkTarget();
}
