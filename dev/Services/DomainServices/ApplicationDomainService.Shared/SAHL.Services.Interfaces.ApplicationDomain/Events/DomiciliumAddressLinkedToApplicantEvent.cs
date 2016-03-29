using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class DomiciliumAddressLinkedToApplicantEvent : Event
    {
        public int ClientKey { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public int ClientDomiciliumKey { get; protected set; }

        public int ApplicationDomiciliumRoleKey { get; protected set; }

        public DomiciliumAddressLinkedToApplicantEvent(DateTime date, int applicationDomiciliumRoleKey, int clientKey, int applicationNumber, int clientDomiciliumKey)
            : base(date)
        {
            this.ApplicationDomiciliumRoleKey = ApplicationDomiciliumRoleKey;
            this.ClientKey = clientKey;
            this.ApplicationNumber = applicationNumber;
            this.ClientDomiciliumKey = clientDomiciliumKey;
        }
    }
}
