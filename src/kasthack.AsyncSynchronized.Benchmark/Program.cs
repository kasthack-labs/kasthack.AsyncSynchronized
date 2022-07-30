namespace kasthack.AsyncSynchronized.Benchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly, args: args);
}
