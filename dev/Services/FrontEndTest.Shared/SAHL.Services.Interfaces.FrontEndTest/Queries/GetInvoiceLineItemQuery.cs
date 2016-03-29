using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetInvoiceLineItemQuery : ServiceQuery<InvoiceLineItemDataModel>, IFrontEndTestQuery, ISqlServiceQuery<InvoiceLineItemDataModel>
    {
        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetInvoiceLineItemQuery(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
