using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;

namespace SAHL.Workflow.Shared.Specs.WorkflowAssignmentSpecs.ReturnCaseToLastUserInCapabilitySpecs
{
    public class when_assignment_succeeds : WithFakes
    {
        private static WorkflowAssignment workflowAssignment;
        private static IWorkflowAssignmentDomainServiceClient serviceClient;
        private static long instanceId;
        private static int userOrganisationStructureKey;
        private static Capability capability;
        private static GenericKeyType genericKeyType;
        private static int genericKey;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static bool result;
        private static ISystemMessageCollection messages;

        private Establish that = () =>
        {
            capability = Capability.InvoiceProcessor;
            genericKeyType = GenericKeyType.ThirdPartyInvoice;
            genericKey = 2;
            instanceId = 3L;
            userOrganisationStructureKey = 4;
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            serviceRequestMetadata.WhenToldTo(x => x.UserOrganisationStructureKey).Return(userOrganisationStructureKey);
            messages = An<ISystemMessageCollection>();
            messages.WhenToldTo(a => a.HasErrors).Return(false);

            serviceClient = An<IWorkflowAssignmentDomainServiceClient>();
            serviceClient
                .WhenToldTo(a => a.PerformCommand(Arg.Any<ReturnInstanceToLastUserInCapabilityCommand>(), serviceRequestMetadata))
                .Return(messages);

            workflowAssignment = new WorkflowAssignment(serviceClient);
        };

        private Because of = () =>
        {
            result = workflowAssignment.ReturnCaseToLastUserInCapability(messages, genericKeyType, genericKey, instanceId, capability, serviceRequestMetadata);
        };

        private It should_have_a_positive_result = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_have_attempted_to_perform_an_AssignWorkflowCaseCommand_on_the_service_client = () =>
        {
            serviceClient
                .WasToldTo(a => a.PerformCommand(Param<ReturnInstanceToLastUserInCapabilityCommand>.Matches(b =>
                    b.GenericKeyTypeKey == genericKeyType
                    && b.GenericKey == genericKey
                    && b.InstanceId == instanceId
                    && b.Capability == capability), serviceRequestMetadata))
                .OnlyOnce();
        };
    }
}