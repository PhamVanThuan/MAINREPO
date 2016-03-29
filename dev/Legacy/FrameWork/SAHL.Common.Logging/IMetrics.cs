using System;
using System.Collections.Generic;
using SAHL.Shared.Messages;
using SAHL.Shared.Messages.Metrics;

namespace SAHL.Common.Logging
{
    public interface IMetrics
    {
        void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source);

        void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source, Dictionary<string, object> parameters);

        void PublishInstantaneousValueMetric(int value, string source);

        void PublishInstantaneousValueMetric(int value, string source, Dictionary<string, object> parameters);

        void PublishThroughputMetric(string source, IEnumerable<TimeUnit> throughputRates);

        void PublishThroughputMetric(string source, IEnumerable<TimeUnit> throughputRates, Dictionary<string, object> parameters);
    }
}
