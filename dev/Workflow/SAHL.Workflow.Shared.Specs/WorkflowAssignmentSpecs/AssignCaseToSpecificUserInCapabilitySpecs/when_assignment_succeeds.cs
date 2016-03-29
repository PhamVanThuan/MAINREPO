using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;

namespace SAHL.Workflow.Shared.Specs.WorkflowAssignmentSpecs.AssignCaseToSpecificUserInCapabilitySpecs
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
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            userOrganisationStructureKey = 4;
            genericKeyType = GenericKeyType.ThirdPartyInvoice;
            genericKey = 2;
            instanceId = 3L;
            capability = Capability.InvoiceProcessor;
            serviceRequestMetadata.WhenToldTo(x => x.UserOrganisationStructureKey).Return(userOrganisationStructureKey);
            messages = An<ISystemMessageCollection>();
            messages.WhenToldTo(a => a.HasErrors).Return(false);

            serviceClient = An<IWorkflowAssignmentDomainServiceClient>();
            serviceClient
                .WhenToldTo(a => a.PerformCommand(Arg.Any<AssignWorkflowCaseCommand>(), serviceRequestMetadata))
                .Return(messages);

            workflowAssignment = new WorkflowAssignment(serviceClient);
        };

        private Because of = () =>
        {
            result = workflowAssignment.AssignCaseToSpecificUserInCapability(messages, genericKeyType,
                genericKey,
                instanceId,
                capability,
                serviceRequestMetadata,
                userOrganisationStructureKey);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}