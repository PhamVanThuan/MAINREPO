using System;
using System.Collections.Generic;

namespace SAHL.Common.Logging
{
    public class DefaultMetrics : IMetrics
    {
        public void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source)
        {
        }

        public void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source, Dictionary<string, object> parameters)
        {
        }

        public void PublishInstantaneousValueMetric(int value, string source)
        {
        }

        public void PublishInstantaneousValueMetric(int value, string source, Dictionary<string, object> parameters)
        {
        }

        public void PublishThroughputMetric(string source, IEnumerable<Shared.Messages.Metrics.TimeUnit> throughputRates)
        {
        }

        public void PublishThroughputMetric(string source, IEnumerable<Shared.Messages.Metrics.TimeUnit> throughputRates, Dictionary<string, object> parameters)
        {
        }
    }
}