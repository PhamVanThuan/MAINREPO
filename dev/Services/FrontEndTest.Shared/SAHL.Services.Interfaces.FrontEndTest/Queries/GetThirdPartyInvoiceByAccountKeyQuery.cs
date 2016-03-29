using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetThirdPartyInvoiceByAccountKeyQuery : ServiceQuery<GetThirdPartyInvoiceByAccountKeyQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetThirdPartyInvoiceByAccountKeyQueryResult>
    {
        public GetThirdPartyInvoiceByAccountKeyQuery(int accountKey) 
        {
            AccountKey = accountKey;
        }
        public int AccountKey { get; protected set; }
    }
}
