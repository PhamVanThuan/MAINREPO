using System;
using System.Collections.Generic;

using SAHL.Core.Data.Models.Cuttlefish;

namespace SAHL.Core.Logging.Loggers.Database
{
    public class DatabaseMetricsLoggerDataManager : IDatabaseMetricsLoggerDataManager
    {
        private IDatabaseLoggerUtils databaseLoggerUtils;

        public DatabaseMetricsLoggerDataManager(IDatabaseLoggerUtils databaseLoggerUtils)
        {
            this.databaseLoggerUtils = databaseLoggerUtils;
        }

        public void StoreLatencyMetricLog(string source, string applicationName, string user, string metricName, DateTime startTime, 
                                          TimeSpan duration, IDictionary<string, object> parameters)
        {
            var machineName = Environment.MachineName;
            var latencyMetricMessage = new LatencyMetricMessageDataModel(startTime, duration.Ticks, source, user, DateTime.Now, machineName, applicationName, metricName);

            databaseLoggerUtils.LogTaskManager.QueueTask(new Action(() =>
            {
                databaseLoggerUtils.DatabaseInsert<LatencyMetricMessageDataModel>(latencyMetricMessage, parameters);
            }));
        }

        public void StoreInstantaneousValueMetricLog(int? instantaneousValue, string source, string userName, string application, IDictionary<string, object> parameters)
        {
            var machineName = Environment.MachineName;
            var instantaneousValueMetricMessage = new InstantaneousValueMetricMessageDataModel(instantaneousValue, source, userName, DateTime.Now, machineName, application);

            databaseLoggerUtils.LogTaskManager.QueueTask(new Action(() =>
            {
                databaseLoggerUtils.DatabaseInsert<InstantaneousValueMetricMessageDataModel>(instantaneousValueMetricMessage, parameters);
            }));
        }

        public void StoreThroughputMetricLog(string source, string userName, DateTime? messageDate, string application, string metric, IDictionary<string, object> parameters)
        {
            var machineName = Environment.MachineName;
            var throughputMetricMessage = new ThroughputMetricMessageDataModel(source, userName, DateTime.Now, machineName, application, metric);

            databaseLoggerUtils.LogTaskManager.QueueTask(new Action(() =>
            {
                databaseLoggerUtils.DatabaseInsert<ThroughputMetricMessageDataModel>(throughputMetricMessage, parameters);
            }));
        }
    }
}