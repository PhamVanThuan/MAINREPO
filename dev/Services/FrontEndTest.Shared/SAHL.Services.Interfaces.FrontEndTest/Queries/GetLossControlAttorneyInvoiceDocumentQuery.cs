using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetLossControlAttorneyInvoiceDocumentQuery : ServiceQuery<string>, IFrontEndTestQuery
    {
        public int ThirdPartyInvoiceKey { get; set; }
        public GetLossControlAttorneyInvoiceDocumentQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
