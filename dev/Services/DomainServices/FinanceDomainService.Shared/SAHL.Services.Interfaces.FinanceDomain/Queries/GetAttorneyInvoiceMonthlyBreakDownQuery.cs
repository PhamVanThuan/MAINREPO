using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetAttorneyInvoiceMonthlyBreakDownQuery :
        ServiceQuery<AttorneyInvoiceMonthlyBreakdownDataModel>, IFinanceDomainQuery, ISqlServiceQuery<AttorneyInvoiceMonthlyBreakdownDataModel>
    {
        public GetAttorneyInvoiceMonthlyBreakDownQuery()
        {
        }
    }
}
