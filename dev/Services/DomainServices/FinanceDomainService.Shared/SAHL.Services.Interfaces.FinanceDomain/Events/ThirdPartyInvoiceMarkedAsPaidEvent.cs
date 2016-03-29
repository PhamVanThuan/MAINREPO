using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceMarkedAsPaidEvent : Event
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ThirdPartyInvoiceMarkedAsPaidEvent(DateTime date, int thirdPartyInvoiceKey) : base(date)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}