namespace kasthack.AsyncSynchronized.Tests.Targets
{
    using Xunit.Abstractions;

    public interface ITestTarget
    {
        int CallCount { get; }

        int ID { get; }

        ITestOutputHelper? Logger { get; set; }

        int ReturnCount { get; }

        int ThreadCount { get; }

        Task ThreadUnsafe();
    }
}