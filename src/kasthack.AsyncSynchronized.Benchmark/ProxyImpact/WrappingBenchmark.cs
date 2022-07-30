namespace kasthack.AsyncSynchronized.Benchmark.ProxyImpact
{
    using BenchmarkDotNet.Attributes;

    using kasthack.AsyncSynchronized.Benchmark.Targets;

    public class WrappingBenchmark
    {
        private readonly BenchmarkTarget source;

        public WrappingBenchmark()
        {
            this.source = new BenchmarkTarget();
        }

        [Benchmark]
        public void Wrap() => this.source.Synchronized();
    }
}
