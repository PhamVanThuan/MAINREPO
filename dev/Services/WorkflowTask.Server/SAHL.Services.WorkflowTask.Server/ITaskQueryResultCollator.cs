using System.Collections.Generic;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;

namespace SAHL.Services.WorkflowTask.Server
{
    public interface ITaskQueryResultCollator
    {
        IEnumerable<IDictionary<string, object>> Results { get; set; }
        TaskBaseStatement CreateTaskStatement();
    }
}