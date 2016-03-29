using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public interface IDatabaseMetricsLoggerDataManager
    {
        void StoreInstantaneousValueMetricLog(int? instantaneousValue, string source, string userName, string application, IDictionary<string, object> parameters);

        void StoreLatencyMetricLog(string source, string applicationName, string user, string metricName, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters);

        void StoreThroughputMetricLog(string source, string userName, DateTime? messageDate, string application, string metric, IDictionary<string, object> parameters);
    }
}