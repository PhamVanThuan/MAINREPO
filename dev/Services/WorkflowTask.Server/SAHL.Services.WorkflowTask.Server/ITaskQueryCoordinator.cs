using System.Collections.Generic;
using SAHL.Services.Interfaces.WorkflowTask;

namespace SAHL.Services.WorkflowTask.Server
{
    public interface ITaskQueryCoordinator
    {
        IEnumerable<BusinessProcess> GetWorkflowTasks(string username, List<string> roles);
    }
}