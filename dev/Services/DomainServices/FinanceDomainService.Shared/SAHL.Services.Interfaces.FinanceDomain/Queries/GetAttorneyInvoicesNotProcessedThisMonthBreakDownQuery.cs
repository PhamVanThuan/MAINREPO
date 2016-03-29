using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetAttorneyInvoicesNotProcessedThisMonthBreakDownQuery : ServiceQuery<AttorneyInvoicesNotProcessedThisMonthDataModel>,
        IFinanceDomainQuery, ISqlServiceQuery<AttorneyInvoicesNotProcessedThisMonthDataModel>
    {
        public GetAttorneyInvoicesNotProcessedThisMonthBreakDownQuery()
        {
        }
    }
}
