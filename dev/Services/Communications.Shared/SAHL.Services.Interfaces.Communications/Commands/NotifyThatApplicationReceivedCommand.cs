using SAHL.Core.Services;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Communications.Commands
{
    public class NotifyThatApplicationReceivedCommand : ServiceCommand, ICommunicationsServiceCommand
    {
        public List<Recipient> Recipients { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public NotifyThatApplicationReceivedCommand(List<Recipient> recipients, int applicationNumber)
        {
            this.Recipients = recipients;
            this.ApplicationNumber = applicationNumber;
        }
    }
}