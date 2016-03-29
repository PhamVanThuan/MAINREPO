using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;

namespace SAHL.Workflow.Shared.Specs.WorkflowAssignmentSpecs.NotifyOfChangeToStaticAssignmentSpecs
{
    public class when_notification_succeeds : WithFakes
    {
        private static WorkflowAssignment workflowAssignment;
        private static IWorkflowAssignmentDomainServiceClient serviceClient;
        private static long instanceId;
        private static string staticRoleName;
        private static GenericKeyType genericKeyType;
        private static int genericKey;
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static bool result;
        private static ISystemMessageCollection messages;

        private Establish that = () =>
        {
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            staticRoleName = "Invoice Processor";
            genericKeyType = GenericKeyType.ThirdPartyInvoice;
            genericKey = 2;
            instanceId = 3L;
            messages = An<ISystemMessageCollection>();
            messages.WhenToldTo(a => a.HasErrors).Return(false);
            serviceClient = An<IWorkflowAssignmentDomainServiceClient>();
            serviceClient.WhenToldTo(a => a.PerformCommand(Arg.Any<NotificationOfInstanceStaticWorkflowAssignmentCommand>(), serviceRequestMetadata))
                .Return(messages);
            workflowAssignment = new WorkflowAssignment(serviceClient);
        };

        private Because of = () =>
        {
            result = workflowAssignment.NotifyOfChangeToStaticAssignment(messages, genericKeyType, genericKey, instanceId, serviceRequestMetadata, staticRoleName);
        };

        private It should_return_a_positive_result = () =>
        {
            result.ShouldBeTrue();
        };
    }
}