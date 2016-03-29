using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification.Constants;

namespace SAHL.Services.Communications.CommandHandlers
{
    public class NotifyThatApplicationReceivedCommandHandler : IServiceCommandHandler<NotifyThatApplicationReceivedCommand>
    {
        private INotificationService notificationService;

        public NotifyThatApplicationReceivedCommandHandler(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public ISystemMessageCollection HandleCommand(NotifyThatApplicationReceivedCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messageCollection = SystemMessageCollection.Empty();
            messageCollection.Aggregate(this.notificationService.NotifyRecipients(command.Recipients, 
                Notifications.UpdateApplicationRecievedNotification(command.ApplicationNumber)));
            return messageCollection;
        }
    }
}