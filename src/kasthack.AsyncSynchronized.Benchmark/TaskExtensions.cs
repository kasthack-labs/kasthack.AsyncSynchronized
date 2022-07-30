public static class TaskExtensions
{
    #region Task -> valuetask
    public static async ValueTask FastContinueWith(this Task task, Action<Task> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        continuation(task);
    }
    public static async ValueTask FastContinueWith<TInput>(this Task<TInput> task, Action<Task<TInput>> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        continuation(task);
    }
    public static async ValueTask<TOutput> FastContinueWith<TOutput>(this Task task, Func<Task, TOutput> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        return continuation(task);
    }
    public static async ValueTask<TOutput> FastContinueWith<TInput, TOutput>(this Task<TInput> task, Func<Task<TInput>, TOutput> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        return continuation(task);
    }
    #endregion
    #region ValueTask -> valuetask
    public static async ValueTask FastContinueWith(this ValueTask task, Action<ValueTask> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        continuation(task);
    }
    public static async ValueTask FastContinueWith<TInput>(this ValueTask<TInput> task, Action<ValueTask<TInput>> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        continuation(task);
    }
    public static async ValueTask<TOutput> FastContinueWith<TOutput>(this ValueTask task, Func<ValueTask, TOutput> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        return continuation(task);
    }
    public static async ValueTask<TOutput> FastContinueWith<TInput, TOutput>(this ValueTask<TInput> task, Func<ValueTask<TInput>, TOutput> continuation)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch { }
        return continuation(task);
    }
    #endregion

    public static async ValueTask FastUnwrap(this ValueTask<ValueTask> task)
    {
        try { await task; }
        catch { }
        await task.Result;
    }
    public static async ValueTask FastUnwrap(this ValueTask<Task> task)
    {
        try { await task; }
        catch { }
        await task.Result;
    }
    public static async ValueTask FastUnwrap(this Task<Task> task)
    {
        try { await task; }
        catch { }
        await task.Result;
    }
    public static async ValueTask FastUnwrap(this Task<ValueTask> task)
    {
        try { await task; }
        catch { }
        await task.Result;
    }


    public static async ValueTask<T> FastUnwrap<T>(this ValueTask<ValueTask<T>> task)
    {
        try { await task; }
        catch { }
        return await task.Result;
    }
    public static async ValueTask<T> FastUnwrap<T>(this ValueTask<Task<T>> task)
    {
        try { await task; }
        catch { }
        return await task.Result;
    }
    public static async ValueTask<T> FastUnwrap<T>(this Task<Task<T>> task)
    {
        try { await task; }
        catch { }
        return await task.Result;
    }
    public static async ValueTask<T> FastUnwrap<T>(this Task<ValueTask<T>> task)
    {
        try { await task; }
        catch { }
        return await task.Result;
    }
}