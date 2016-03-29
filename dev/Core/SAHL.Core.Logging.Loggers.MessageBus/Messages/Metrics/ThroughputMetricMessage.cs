using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Messages.Metrics
{
    [Serializable]
    public class ThroughputMetricMessage : MetricMessage
    {
        private List<TimeUnitReference> throughputRates;

        public ThroughputMetricMessage(IEnumerable<TimeUnit> throughputRates, string source, string user = "")
            : base(source, user)
        {
            this.throughputRates = new List<TimeUnitReference>();

            foreach (var timeUnit in throughputRates)
            {
                this.throughputRates.Add(new TimeUnitReference(timeUnit));
            }
        }

        public IEnumerable<TimeUnitReference> ThroughputRates { get { return this.throughputRates; } }
    }
}