using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public interface IDatabaseLoggerDataManager
    {
        void StoreLog(string logMessageType, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null);
    }
}