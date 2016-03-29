using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec.ExternalServiceModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.ExternalServices.Notification
{
    public interface INotificationService
    {
        ISystemMessageCollection NotifyRecipients(List<Recipient> recipients, string message);
	}
}
