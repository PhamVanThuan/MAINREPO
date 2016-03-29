using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.Maps.Config;

namespace SAHL.Workflow.Shared
{
    public interface IWorkflowAssignment : IWorkflowService
    {
        bool AssignCaseToUserInCapability(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, Capability capability,
            IServiceRequestMetadata serviceRequestMetadata);

        string ResolveUserInCapability(ISystemMessageCollection messages, long instanceId, Capability capability);

        bool ReturnCaseToLastUserInCapability(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, Capability capability,
            IServiceRequestMetadata serviceRequestMetadata);

        bool AssignCaseToSpecificUserInCapability(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, Capability capability,
            IServiceRequestMetadata serviceRequestMetadata, int userOrganisationStructureKey);

        bool NotifyOfChangeToStaticAssignment(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, IServiceRequestMetadata serviceRequestMetadata,
            string staticGroupName);
    }
}