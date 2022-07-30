namespace kasthack.AsyncSynchronized.Benchmark.ImplementationComparison;

using BenchmarkDotNet.Attributes;

using kasthack.AsyncSynchronized.Benchmark.Targets;

public class ImplementationComparisonBenchmark
{
    private const int Loops = 10_000;

    protected BenchmarkTarget RawTarget { get; } = new BenchmarkTarget();

    protected BenchmarkTarget ManualTarget { get; } = new SyncBenchmarkTarget();

    protected BenchmarkTarget SyncTarget { get; } = new BenchmarkTarget().Synchronized();

    protected BenchmarkTarget CachedLambdasTarget { get; } = new CachedLambdasBenchmarkTarget();

    protected BenchmarkTarget CachedLambdasNonReadonlyTarget { get; } = new CachedLambdasNonReadonlyBenchmarkTarget();

    protected BenchmarkTarget CachedLambdasAllTarget { get; } = new CachedLambdasAllBenchmarkTarget();

    protected BenchmarkTarget StateMachineTarget { get; } = new StateMachineBenchmarkTarget();

    protected BenchmarkTarget StateMachineCAFalseTarget { get; } = new StateMachineCAFalseBenchmarkTarget();

    protected BenchmarkTarget LessCallTarget { get; } = new SampleCallsTarget();

    protected BenchmarkTarget CachedLambdasNCTarget { get; } = new CachedLambdasNCBenchmarkTarget();
    protected BenchmarkTarget FastContinueTarget { get; } = new FastContinueTarget();

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask Raw() => await this.Benchmark(this.RawTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask ManualPre() => await this.Benchmark(this.ManualTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask CachedLambdas() => await this.Benchmark(this.CachedLambdasTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask CachedLambdasNonReadonly() => await this.Benchmark(this.CachedLambdasNonReadonlyTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask CachedLambdasAll() => await this.Benchmark(this.CachedLambdasAllTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask StateMachine() => await this.Benchmark(this.StateMachineTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask StateMachineCAFalse() => await this.Benchmark(this.StateMachineCAFalseTarget).ConfigureAwait(false);
    
    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask LessCalls() => await this.Benchmark(this.LessCallTarget).ConfigureAwait(false);

    [Benchmark(OperationsPerInvoke = Loops)]
    public ValueTask<int> NoClosures() => this.Benchmark(this.CachedLambdasNCTarget);

    [Benchmark(OperationsPerInvoke = Loops)]
    public ValueTask<int> FastContinue() => this.Benchmark(this.FastContinueTarget);

    //is my laptop getting trottled?
    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask ManualPost() => await this.Benchmark(this.ManualTarget).ConfigureAwait(false);

    //[Benchmark(OperationsPerInvoke = Loops)]
    public async ValueTask Sync() => await this.Benchmark(this.SyncTarget).ConfigureAwait(false);

    public async ValueTask<int> Benchmark(BenchmarkTarget target)
    {
        var result = 0;
        for (int i = 0; i < Loops; i++)
        {
            result += await target.TaskWithResult().ConfigureAwait(false);
        }

        return result;
    }
}
