namespace kasthack.AsyncSynchronized.Benchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main() => BenchmarkRunner.Run(typeof(Program).Assembly);
}
