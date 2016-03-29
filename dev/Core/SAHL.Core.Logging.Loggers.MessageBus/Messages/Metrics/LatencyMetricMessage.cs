using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Messages.Metrics
{
    [Serializable]
    public class LatencyMetricMessage : MetricMessage
    {
        public LatencyMetricMessage(string source, DateTime startTime, TimeSpan duration, string user = "", IDictionary<string, object> parameters = null)
            : base(source, user, parameters)
        {
            this.StartTime = startTime;
            this.Duration = duration;
        }

        public virtual DateTime StartTime { get; protected set; }

        public virtual TimeSpan Duration { get; protected set; }
    }
}