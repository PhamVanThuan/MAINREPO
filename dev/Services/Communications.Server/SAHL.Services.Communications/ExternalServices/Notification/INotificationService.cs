using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using System.Collections.Generic;

namespace SAHL.Services.Communications.ExternalServices.Notification
{
    public interface INotificationService
    {
        ISystemMessageCollection NotifyRecipients(IEnumerable<Recipient> recipients, string message);
    }
}