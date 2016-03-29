using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public class DatabaseMetricsLogger : IRawMetricsLogger
    {
        private IDatabaseMetricsLoggerDataManager databaseMetricsLoggerDataManager;
        private IDatabaseLoggerUtils databaseLoggerUtils;

        public DatabaseMetricsLogger(IDatabaseMetricsLoggerDataManager databaseMetricsLoggerDataManager, IDatabaseLoggerUtils databaseLoggerUtils)
        {
            this.databaseMetricsLoggerDataManager = databaseMetricsLoggerDataManager;
            this.databaseLoggerUtils = databaseLoggerUtils;
        }

        public void LogLatencyMetric(string source, string application, string user, string metric, DateTime startTime, TimeSpan duration)
        {
            this.databaseMetricsLoggerDataManager.StoreLatencyMetricLog(source, application, user, metric, startTime, duration, null);
        }

        public void LogLatencyMetric(string source, string application, string user, string metric, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);
            this.databaseMetricsLoggerDataManager.StoreLatencyMetricLog(source, application, user, metric, startTime, duration, parameters);
        }

        public void LogInstantaneousValueMetric(string source, string application, string user, string metric, int value)
        {
            this.databaseMetricsLoggerDataManager.StoreInstantaneousValueMetricLog(value, source, user, application, null);
        }

        public void LogInstantaneousValueMetric(string source, string application, string user, string metric, int value, IDictionary<string, object> parameters)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);
            this.databaseMetricsLoggerDataManager.StoreInstantaneousValueMetricLog(value, source, user, application, parameters);
        }

        public void LogThroughputMetric(string source, string application, string user, string metric)
        {
            this.databaseMetricsLoggerDataManager.StoreThroughputMetricLog(source, user, DateTime.Now, application, metric, null);
        }

        public void LogThroughputMetric(string source, string application, string user, string metric, IDictionary<string, object> parameters)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);
            this.databaseMetricsLoggerDataManager.StoreThroughputMetricLog(source, user, DateTime.Now, application, metric, parameters);
        }

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return Logger.ThreadContext;
        }

        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            foreach (var kvp in threadContext)
            {
                if (!Logger.ThreadContext.ContainsKey(kvp.Key))
                {
                    Logger.ThreadContext.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    threadContext[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}