using System.Collections.Generic;

namespace SAHL.Shared.Messages.Metrics
{
    public class InstantaneousValueMetricMessage : MetricBaseMessage
    {
        protected InstantaneousValueMetricMessage()
        {

        }

        public InstantaneousValueMetricMessage(string application, string source, int instantaneousValue, string user = "", Dictionary<string, object> parameters = null)
            : base(application, source, user, parameters)
        {
            this.InstantaneousValue = instantaneousValue;
        }

        public virtual int InstantaneousValue { get; protected set; }
    }
}