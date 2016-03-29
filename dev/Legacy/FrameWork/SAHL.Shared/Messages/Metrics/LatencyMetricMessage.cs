using System;
using System.Collections.Generic;

namespace SAHL.Shared.Messages.Metrics
{
    [Serializable]
    public class LatencyMetricMessage : MetricBaseMessage
    {
        protected LatencyMetricMessage(){}
		public LatencyMetricMessage(string application, string source, DateTime startTime, TimeSpan duration, string machineName, string user = "", Dictionary<string, object> parameters = null)
			: base(application, source, machineName, user, parameters)
		{
			this.StartTime = startTime;
			this.Duration = duration;
		}
        public LatencyMetricMessage(string application, string source, DateTime startTime, TimeSpan duration, string user = "", Dictionary<string, object> parameters = null)
            : base(application, source, user, parameters)
        {
            this.StartTime = startTime;
            this.Duration = duration;
        }

        public virtual DateTime StartTime { get; protected set; }

        public virtual TimeSpan Duration { get; protected set; }
    }
}