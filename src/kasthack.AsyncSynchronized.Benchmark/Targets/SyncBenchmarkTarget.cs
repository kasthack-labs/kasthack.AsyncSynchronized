namespace kasthack.AsyncSynchronized.Benchmark.Targets;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


public class FastContinueTarget : BenchmarkTarget
{
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .FastContinueWith(this.TaskWithResultFunction)
            .FastUnwrap()
            .FastContinueWith(this.ReleaseSempahoreAndReturn)
            .AsTask();
    }

    private Task<int> TaskWithResultFunction(Task t)
    {
        return base.TaskWithResult();
    }

    private T ReleaseSempahoreAndReturn<T>(ValueTask<T> task)
    {
        this._semaphore.Release();
        return task.Result;
    }
}


public class SampleCallsTarget : BenchmarkTarget
{
    protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .ContinueWith(a => 1)
            .ContinueWith(this.ReleaseSempahoreAndReturn);
    }

    private T ReleaseSempahoreAndReturn<T>(Task<T> task)
    {
        this._semaphore.Release();
        return task.Result;
    }
}

public class CachedLambdasNCBenchmarkTarget : BenchmarkTarget
{
    protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .ContinueWith(this.TaskWithResultFunction)
            .Unwrap()
            .ContinueWith(this.ReleaseSempahoreAndReturn);
    }

    private Task<int> TaskWithResultFunction(Task t)
    {
        return base.TaskWithResult();
    }

    private T ReleaseSempahoreAndReturn<T>(Task<T> task)
    {
        this._semaphore.Release();
        return task.Result;
    }
}

public class CachedLambdasBenchmarkTarget : BenchmarkTarget
{
    protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    private Func<Task, Task<int>> taskWithResultFunction;

    public CachedLambdasBenchmarkTarget()
    {
        this.taskWithResultFunction = _ => base.TaskWithResult();
    }

    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .ContinueWith(this.taskWithResultFunction)
            .Unwrap()
            .ContinueWith(this.ReleaseSempahoreAndReturn);
    }

    private T ReleaseSempahoreAndReturn<T>(Task<T> task)
    {
        this._semaphore.Release();
        return task.Result;
    }
}

public class CachedLambdasNonReadonlyBenchmarkTarget : BenchmarkTarget
{
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    private Func<Task, Task<int>> taskWithResultFunction;

    public CachedLambdasNonReadonlyBenchmarkTarget()
    {
        this.taskWithResultFunction = _ => base.TaskWithResult();
    }

    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .ContinueWith(this.taskWithResultFunction)
            .Unwrap()
            .ContinueWith(this.ReleaseSempahoreAndReturn);
    }

    private T ReleaseSempahoreAndReturn<T>(Task<T> task)
    {
        this._semaphore.Release();
        return task.Result;
    }
}
public class StateMachineBenchmarkTarget : BenchmarkTarget
{
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    private Func<Task, Task<int>> taskWithResultFunction;

    public StateMachineBenchmarkTarget()
    {
        this.taskWithResultFunction = _ => base.TaskWithResult();
    }

    public override async Task<int> TaskWithResult()
    {
        try
        {
            await _semaphore.WaitAsync();
            return await base.TaskWithResult();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
public class StateMachineCAFalseBenchmarkTarget : BenchmarkTarget
{
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    private Func<Task, Task<int>> taskWithResultFunction;

    public StateMachineCAFalseBenchmarkTarget()
    {
        this.taskWithResultFunction = _ => base.TaskWithResult();
    }

    public override async Task<int> TaskWithResult()
    {
        try
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            return await base.TaskWithResult().ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
public class CachedLambdasAllBenchmarkTarget : BenchmarkTarget
{
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    private Func<Task, Task<int>> taskWithResultFunction;
    private Func<Task<int>, int> releaseSempahoreAndReturn;

    public CachedLambdasAllBenchmarkTarget()
    {
        this.taskWithResultFunction = _ => base.TaskWithResult();
        this.releaseSempahoreAndReturn = task =>
        {
            this._semaphore.Release();
            return task.Result;
        };
    }

    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .ContinueWith(this.taskWithResultFunction)
            .Unwrap()
            .ContinueWith(this.releaseSempahoreAndReturn);
    }

}

public class SyncBenchmarkTarget : BenchmarkTarget
{
    protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

    public override async Task AsyncTask()
    {
        try
        {
            await this._semaphore.WaitAsync().ConfigureAwait(false);
            await base.AsyncTask().ConfigureAwait(false);
        }
        finally
        {
            this._semaphore.Release();
        }
    }

    public override async Task<int> AsyncTaskWithResult()
    {
        try
        {
            await this._semaphore.WaitAsync().ConfigureAwait(false);
            return await base.AsyncTaskWithResult().ConfigureAwait(false);
        }
        finally
        {
            this._semaphore.Release();
        }
    }

    public override int Int()
    {
        try
        {
            this._semaphore.Wait();
            return base.Int();
        }
        finally
        {
            this._semaphore.Release();
        }
    }

    public override int Property
    {
        get
        {
            try
            {
                this._semaphore.Wait();
                return base.Property;
            }
            finally
            {
                this._semaphore.Release();
            }
        }
    }

    public override Task<int> TaskWithResult()
    {
        //TODO:
        //investigate perf
        return this._semaphore
            .WaitAsync()
            .ContinueWith(_ => base.TaskWithResult())
            .Unwrap()
            .ContinueWith(task =>
            {
                this._semaphore.Release();
                return task.Result;
            });
    }

    public override void Void()
    {
        try
        {
            this._semaphore.Wait();
            base.Void();
        }
        finally
        {
            this._semaphore.Release();
        }
    }
}