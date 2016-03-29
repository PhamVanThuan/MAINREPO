using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetAttorneyInvoicesPaidPreviousMonthBreakDownQuery : ServiceQuery<AttorneyInvoicesPaidLastMonthDataModel>,
        IFinanceDomainQuery, ISqlServiceQuery<AttorneyInvoicesPaidLastMonthDataModel>
    {
        public GetAttorneyInvoicesPaidPreviousMonthBreakDownQuery()
        {
        }
    }
}
