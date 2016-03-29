using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.WorkflowAssignmentDataManagerSpecs
{
    public interface IWorkflowAssignmentDataManager
    {
        bool DoesCapabilityExist(int instanceId);
    }
}
