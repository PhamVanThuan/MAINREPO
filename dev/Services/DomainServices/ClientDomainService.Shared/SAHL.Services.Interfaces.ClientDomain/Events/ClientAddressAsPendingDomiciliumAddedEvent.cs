using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class ClientAddressAsPendingDomiciliumAddedEvent : Event
    {
        public ClientAddressAsPendingDomiciliumAddedEvent(DateTime date, int legalEntityAddressKey, int clientDomiciliumKey)
            : base(date)
        {
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.ClientDomiciliumKey = clientDomiciliumKey;
        }

        public int LegalEntityAddressKey { get; protected set; }

        public int ClientDomiciliumKey { get; protected set; }
    }
}