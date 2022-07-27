namespace kasthack.AsyncSynchronized.Tests.Targets
{
    using Xunit.Abstractions;

    public class InterfacedTestTarget : TestTarget
    {
        public InterfacedTestTarget(int preventsCastleClassProxyFromWorking)
        {
        }
    }
}