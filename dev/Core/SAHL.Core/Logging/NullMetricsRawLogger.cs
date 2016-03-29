using System;
using System.Collections.Generic;
using System.Threading;

namespace SAHL.Core.Logging
{
    public class NullMetricsRawLogger : IRawMetricsLogger
    {
        private static ThreadLocal<IDictionary<string, object>> threadContext = new ThreadLocal<IDictionary<string, object>>(() => new Dictionary<string, object>());

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return NullMetricsRawLogger.threadContext.Value;
        }

        public void LogInstantaneousValueMetric(string source, string application, string user, string metric, int value)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogInstantaneousValueMetric(string source, string application, string user, string metric, int value, IDictionary<string, object> parameters)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogLatencyMetric(string source, string application, string user, string metric, DateTime startTime, TimeSpan duration)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogLatencyMetric(string source, string application, string user, string metric, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogThroughputMetric(string source, string application, string user, string metric)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogThroughputMetric(string source, string application, string user, string metric, IDictionary<string, object> parameters)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            NullMetricsRawLogger.threadContext.Value = threadContext;
        }
    }
}