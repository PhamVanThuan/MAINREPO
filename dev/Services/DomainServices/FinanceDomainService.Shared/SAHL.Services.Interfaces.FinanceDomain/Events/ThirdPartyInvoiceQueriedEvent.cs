using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceQueriedEvent : Event
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public string QueryInitiatedBy { get; protected set; }

        public string QueryComments { get; protected set; }

        public ThirdPartyInvoiceQueriedEvent(DateTime date, int thirdPartyInvoiceKey, string queryInitiatedBy, string queryComments)
            : base(date)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.QueryInitiatedBy = queryInitiatedBy;
            this.QueryComments = queryComments;
        }
    }
}