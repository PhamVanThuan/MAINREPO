using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Events;
using System;

namespace SAHL.Services.WorkflowAssignmentDomain.CommandHandlers
{
    public class NotificationOfInstanceStaticWorkflowAssignmentCommandHandler : IServiceCommandHandler<NotificationOfInstanceStaticWorkflowAssignmentCommand>
    {
        private IEventRaiser eventRaiser;

        public NotificationOfInstanceStaticWorkflowAssignmentCommandHandler(IEventRaiser eventRaiser)
        {
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(NotificationOfInstanceStaticWorkflowAssignmentCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var @event = new InstanceStaticWorkflowAssignmentNotifiedEvent(DateTime.Now, command.InstanceId, (int)command.GenericKeyTypeKey, command.StaticGroupName, command.GenericKey);
            eventRaiser.RaiseEvent(DateTime.Now, @event, command.GenericKey, (int)command.GenericKeyTypeKey, metadata);
            return messages;
        }
    }
}