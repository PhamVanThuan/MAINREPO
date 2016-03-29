using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.ExternalServices.Notification;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ExternalServiceModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class NotifyThatApplicationReceivedCommandHandler: IServiceCommandHandler<NotifyThatApplicationReceivedCommand>
    {
        private INotificationService notificationService;

        public NotifyThatApplicationReceivedCommandHandler(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public ISystemMessageCollection HandleCommand(NotifyThatApplicationReceivedCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messageCollection = SystemMessageCollection.Empty();
            var recipients = command.Applicants.Select(x => new Recipient() { CellPhoneNumber = x.Information.CellPhoneNumber }).ToList();
            messageCollection.Aggregate(this.notificationService.NotifyRecipients(recipients, Notifications.UpdateApplicationRecievedNotification(command.ApplicationNumber)));
            return messageCollection;
        }
    }
}
