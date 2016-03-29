namespace SAHL.Core.Logging.Messages.Metrics
{
    public class InstantaneousValueMetricMessage : MetricMessage
    {
        public InstantaneousValueMetricMessage(int instantaneousValue, string source, string user = "")
            : base(source, user)
        {
            this.InstantaneousValue = instantaneousValue;
        }

        public int InstantaneousValue { get; protected set; }
    }
}