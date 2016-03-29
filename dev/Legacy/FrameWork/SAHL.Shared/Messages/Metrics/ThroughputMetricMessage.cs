using System;
using System.Collections.Generic;

namespace SAHL.Shared.Messages.Metrics
{
    [Serializable]
    public class ThroughputMetricMessage : MetricBaseMessage
    {
        private List<TimeUnitReference> throughputRates;

        protected ThroughputMetricMessage()
        {
            this.throughputRates = new List<TimeUnitReference>();
        }

        public ThroughputMetricMessage(string application, string source, IEnumerable<TimeUnit> throughputRates, string user = "", Dictionary<string, object> parameters = null)
            : base(application, source, user, parameters)
        {
            this.throughputRates = new List<TimeUnitReference>();

            foreach (var timeUnit in throughputRates)
            {
                this.throughputRates.Add(new TimeUnitReference(timeUnit));
            }
        }

        public virtual IEnumerable<TimeUnitReference> ThroughputRates { get { return this.throughputRates; } }
    }
}