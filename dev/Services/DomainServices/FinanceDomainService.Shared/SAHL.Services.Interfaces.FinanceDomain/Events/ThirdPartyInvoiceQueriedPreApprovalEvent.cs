using System;
using SAHL.Core.Events;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceQueriedPreApprovalEvent : ThirdPartyInvoiceQueriedEvent
    {
        public ThirdPartyInvoiceQueriedPreApprovalEvent(DateTime date, int thirdPartyInvoiceKey, string queryInitiatedBy, string queryComments)
        :base(date, thirdPartyInvoiceKey, queryInitiatedBy, queryComments)
        {
        }
    }
}