using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using System;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_notifying_of_static_assignment : WithFakes
    {
        private static IEventRaiser eventRaiser;
        private static NotificationOfInstanceStaticWorkflowAssignmentCommandHandler handler;
        private static NotificationOfInstanceStaticWorkflowAssignmentCommand command;
        private static IServiceRequestMetadata metadata;
        private static long instanceId;
        private static GenericKeyType genericKeyType;
        private static int genericKey;
        private static string staticGroupName;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
         {
             messages = SystemMessageCollection.Empty();
             instanceId = 343423123L;
             genericKeyType = GenericKeyType.ThirdPartyInvoice;
             staticGroupName = "Payment Processor";
             genericKey = 456;
             eventRaiser = An<IEventRaiser>();
             command = new NotificationOfInstanceStaticWorkflowAssignmentCommand(instanceId, genericKeyType, staticGroupName, genericKey);
             handler = new NotificationOfInstanceStaticWorkflowAssignmentCommandHandler(eventRaiser);
         };

        private Because of = () =>
         {
             messages = handler.HandleCommand(command, metadata);
         };

        private It should_raise_the_event_with_the_correct_data = () =>
         {
             eventRaiser.Received().RaiseEvent(Arg.Any<DateTime>(), Arg.Is<InstanceStaticWorkflowAssignmentNotifiedEvent>(
                 y => y.StaticGroupName == command.StaticGroupName
                 && y.GenericKey == command.GenericKey
                 && y.InstanceId == command.InstanceId
                 && y.GenericKeyTypeKey == (int)command.GenericKeyTypeKey),
                 command.GenericKey, (int)command.GenericKeyTypeKey, metadata);
         };

        private It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}