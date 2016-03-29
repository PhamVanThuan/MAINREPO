using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Managers
{
    public interface IWorkflowCaseDataManager
    {
        void AssignWorkflowCase(AssignWorkflowCaseCommand command);

        IEnumerable<CapabilityDataModel> GetCapabilitiesForUserOrganisationStructureKey(int userOrganisationStructureKey);

        UserModel GetLastUserAssignedInCapability(int capabilityKey, long instanceId);

        ADUserDataModel GetADUserByUserOrganisationStructureKey(int userOrganisationStructureKey);
    }
}