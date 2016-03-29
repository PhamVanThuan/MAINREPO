using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceReturnedToPaymentQueueEvent : Event
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ThirdPartyInvoiceReturnedToPaymentQueueEvent(DateTime date, int thirdPartyInvoiceKey) : base(date)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}