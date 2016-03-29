using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetMandatedUsersForThirdPartyInvoiceEscalationQuery :
        ServiceQuery<GetMandatedUsersForThirdPartyInvoiceEscalationQueryResult>, IFinanceDomainQuery, ISqlServiceQuery<GetMandatedUsersForThirdPartyInvoiceEscalationQueryResult>
    {
        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetMandatedUsersForThirdPartyInvoiceEscalationQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
