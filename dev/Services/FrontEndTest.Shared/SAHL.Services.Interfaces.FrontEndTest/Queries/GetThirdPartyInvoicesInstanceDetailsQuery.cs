using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetThirdPartyInvoicesInstanceDetailsQuery : ServiceQuery<GetThirdPartyInvoicesInstanceDetailsQueryResult>,
        IFrontEndTestQuery, ISqlServiceQuery<GetThirdPartyInvoicesInstanceDetailsQueryResult>
    {
        public GetThirdPartyInvoicesInstanceDetailsQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public int ThirdPartyInvoiceKey { get; set; }
    }
}