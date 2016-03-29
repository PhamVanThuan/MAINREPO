using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Queries
{
    public class GetThirdPartyInvoiceCorrespondenceQuery : ServiceQuery<GetThirdPartyInvoiceCorrespondenceQueryResult>, IFinanceDomainQuery, ISqlServiceQuery<GetThirdPartyInvoiceCorrespondenceQueryResult>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetThirdPartyInvoiceCorrespondenceQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
