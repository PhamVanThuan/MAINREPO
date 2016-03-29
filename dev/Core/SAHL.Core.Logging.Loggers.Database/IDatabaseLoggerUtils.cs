using SAHL.Core.Data;
using SAHL.Core.Tasks;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public interface IDatabaseLoggerUtils
    {
        TaskManager LogTaskManager { get; set; }

        string SerializeObject(object obj);

        void EnsureParametersCreated(ref IDictionary<string, object> parameters);

        void MergeThreadLocalParameters(ref IDictionary<string, object> parameters);

        void DatabaseInsert<T>(T obj, IDictionary<string, object> parameters = null) where T : IDataModelWithPrimaryKeyId;
    }
}