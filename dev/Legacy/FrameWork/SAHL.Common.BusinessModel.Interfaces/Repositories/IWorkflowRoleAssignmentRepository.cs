using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Globals;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IWorkflowRoleAssignmentRepository
    {

        string ReactivateUserOrRoundRobin(GenericKeyTypes uosGenericKeyType, WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, RoundRobinPointers roundRobinPointer);

    }
}
