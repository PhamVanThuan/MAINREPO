using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetAttorneyInvoicesPaidThisYearBreakDownQuery : ServiceQuery<AttorneyInvoicesPaidThisYearDataModel>,
        IFinanceDomainQuery, ISqlServiceQuery<AttorneyInvoicesPaidThisYearDataModel>
    {
        public GetAttorneyInvoicesPaidThisYearBreakDownQuery()
        {
        }
    }
}
