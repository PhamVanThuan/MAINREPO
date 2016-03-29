using System;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Persistance
{
    public interface IWorkflowPersistanceStore : IDisposable
    {
        void PersistProcess(Process processToPersist);

        Process LoadProcess();
    }
}