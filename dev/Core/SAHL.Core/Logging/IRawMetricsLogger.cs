using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging
{
    public interface IRawMetricsLogger : IParticipatesInThreadLocalStorage
    {
        void LogLatencyMetric(string source, string application, string user, string metric, DateTime startTime, TimeSpan duration);

        void LogLatencyMetric(string source, string application, string user, string metric, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters);

        void LogInstantaneousValueMetric(string source, string application, string user, string metric, int value);

        void LogInstantaneousValueMetric(string source, string application, string user, string metric, int value, IDictionary<string, object> parameters);

        void LogThroughputMetric(string source, string application, string user, string metric);

        void LogThroughputMetric(string source, string application, string user, string metric, IDictionary<string, object> parameters);
    }
}