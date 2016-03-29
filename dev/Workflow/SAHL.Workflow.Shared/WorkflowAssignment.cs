using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;
using System.Linq;

namespace SAHL.Workflow.Shared
{
    public class WorkflowAssignment : IWorkflowAssignment
    {
        private readonly IWorkflowAssignmentDomainServiceClient serviceClient;

        public WorkflowAssignment(IWorkflowAssignmentDomainServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        //TODO: method has a lot of parameters, consider using DTO-style class to contain all the keys
        public bool AssignCaseToUserInCapability(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, Capability capability,
            IServiceRequestMetadata serviceRequestMetadata)
        {
            var command = new AssignWorkflowCaseCommand(genericKeyType,
                genericKey,
                instanceId,
                serviceRequestMetadata.UserOrganisationStructureKey.GetValueOrDefault(),
                capability);

            messages.Aggregate(serviceClient.PerformCommand(command, serviceRequestMetadata));
            return !messages.HasErrors;
        }

        public string ResolveUserInCapability(ISystemMessageCollection messages, long instanceId, Capability capability)
        {
            GetUserCurrentlyAssignedInCapabilityQuery query = new GetUserCurrentlyAssignedInCapabilityQuery(instanceId, (int)capability);
            messages.Aggregate(this.serviceClient.PerformQuery(query));
            return query.Result == null || query.Result.Results == null || query.Result.Results.First() == null ? string.Empty : query.Result.Results.First().UserName;
        }

        public bool ReturnCaseToLastUserInCapability(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, Capability capability,
            IServiceRequestMetadata serviceRequestMetadata)
        {
            var command = new ReturnInstanceToLastUserInCapabilityCommand(genericKeyType, genericKey, capability, instanceId);
            messages.Aggregate(serviceClient.PerformCommand(command, serviceRequestMetadata));
            return !messages.HasErrors;
        }

        public bool AssignCaseToSpecificUserInCapability(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, Capability capability,
            IServiceRequestMetadata serviceRequestMetadata, int userOrganisationStructureKey)
        {
            var command = new AssignWorkflowCaseCommand(genericKeyType,
                genericKey,
                instanceId,
                userOrganisationStructureKey,
                capability);

            messages.Aggregate(serviceClient.PerformCommand(command, serviceRequestMetadata));
            return !messages.HasErrors;
        }

        public bool NotifyOfChangeToStaticAssignment(ISystemMessageCollection messages, GenericKeyType genericKeyType, int genericKey, long instanceId, IServiceRequestMetadata serviceRequestMetadata,
            string staticGroupName)
        {
            var command = new NotificationOfInstanceStaticWorkflowAssignmentCommand(instanceId, genericKeyType, staticGroupName, genericKey);
            messages.Aggregate(serviceClient.PerformCommand(command, serviceRequestMetadata));
            return !messages.HasErrors;
        }
    }
}