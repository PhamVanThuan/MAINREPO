using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class RejectThirdPartyInvoiceCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice
    {
        public RejectThirdPartyInvoiceCommand(int thirdPartyInvoiceKey, string rejectionComments)
        {
            this.RejectionComments = rejectionComments;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        [Required]
        public string RejectionComments { get; protected set; }

        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }
    }
}