namespace kasthack.AsyncSynchronized.Tests.Targets
{
    public class PropertyTarget
    {
        public virtual int Value { get; set; }

        // not virtual on purpose: we need to check if the changes get propagated to the undelying model
        public int GetValue() => this.Value;
    }
}