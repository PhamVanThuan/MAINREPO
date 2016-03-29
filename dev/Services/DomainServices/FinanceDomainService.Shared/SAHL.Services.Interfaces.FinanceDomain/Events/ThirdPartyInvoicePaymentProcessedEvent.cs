using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoicePaymentProcessedEvent : Event
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ThirdPartyInvoicePaymentProcessedEvent(DateTime date, int thirdPartyInvoiceKey) : base(date)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}