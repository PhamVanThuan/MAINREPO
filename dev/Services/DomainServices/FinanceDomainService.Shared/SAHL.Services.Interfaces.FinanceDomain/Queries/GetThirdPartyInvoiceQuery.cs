using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetThirdPartyInvoiceQuery : ServiceQuery<GetThirdPartyInvoiceQueryResult>, IFinanceDomainQuery, ISqlServiceQuery<GetThirdPartyInvoiceQueryResult>
    {
        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetThirdPartyInvoiceQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
