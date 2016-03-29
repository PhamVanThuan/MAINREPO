using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceQueriedPostApprovalEvent : ThirdPartyInvoiceQueriedEvent
    {
        public ThirdPartyInvoiceQueriedPostApprovalEvent(DateTime date, int thirdPartyInvoiceKey, string queryInitiatedBy, string queryComments)
            : base(date, thirdPartyInvoiceKey, queryInitiatedBy, queryComments)
        {
        }
    }
}