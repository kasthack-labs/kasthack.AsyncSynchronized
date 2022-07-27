namespace kasthack.AsyncSynchronized.Tests
{
    using kasthack.AsyncSynchronized;
    using kasthack.AsyncSynchronized.Tests.Targets;

    using Xunit.Abstractions;

    // checks if generated proxies correctly forward values
    public record PropertyAssignmentSanityTests(ITestOutputHelper Logger)
    {
        [Fact]
        public void SettingValueWorks() => this.CheckAccessors(new PropertyTarget());

        [Fact]
#pragma warning disable SA1111, SA1114, SA1009
        public void SettingValueThroughProxyWorks() => this.CheckAccessors(new PropertyTarget().Synchronized(
#if DEBUG
            this.Logger
#endif
        ));
#pragma warning restore SA1111, SA1114, SA1009

        private void CheckAccessors(PropertyTarget target)
        {
            const int value = 5;
            target.Value = value;
            Assert.True(target.Value == value, $"{nameof(target.Value)} has not been assigned properly and set to {target.Value} instead of {value}");
            Assert.True(target.GetValue() == value, $"{nameof(target.GetValue)} returned {target.GetValue()} instead of {value}");
        }
    }
}