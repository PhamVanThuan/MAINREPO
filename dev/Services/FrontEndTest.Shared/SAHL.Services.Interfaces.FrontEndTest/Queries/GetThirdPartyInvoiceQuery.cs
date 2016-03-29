using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetThirdPartyInvoiceQuery : ServiceQuery<ThirdPartyInvoiceDataModel>, IFrontEndTestQuery, ISqlServiceQuery<ThirdPartyInvoiceDataModel>
    {
        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetThirdPartyInvoiceQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
