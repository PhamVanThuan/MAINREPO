using SAHL.Core.Data.Models.Cuttlefish;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public class DatabaseLoggerDataManager : IDatabaseLoggerDataManager
    {
        private IDatabaseLoggerUtils databaseLoggerUtils;

        public DatabaseLoggerDataManager(IDatabaseLoggerUtils databaseLoggerUtils)
        {
            this.databaseLoggerUtils = databaseLoggerUtils;
        }

        public void StoreLog(string logMessageType, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null)
        {
            var machineName = Environment.MachineName;
            var logMessage = new LogMessageDataModel(DateTime.Now, logMessageType, methodName, message, source, user, machineName, applicationName);
            databaseLoggerUtils.LogTaskManager.QueueTask(new Action(() =>
            {
                databaseLoggerUtils.DatabaseInsert<LogMessageDataModel>(logMessage, parameters);
            }));
        }
    }
}