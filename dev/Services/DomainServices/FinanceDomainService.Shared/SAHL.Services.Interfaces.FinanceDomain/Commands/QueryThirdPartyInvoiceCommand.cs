using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class QueryThirdPartyInvoiceCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice
    {
        public QueryThirdPartyInvoiceCommand(int thirdPartyInvoiceKey, string queryComments)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.QueryComments = queryComments;
        }

        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        [Required(ErrorMessage = "Comments are required when querying an invoice.")]
        public string QueryComments { get; protected set; }
    }
}