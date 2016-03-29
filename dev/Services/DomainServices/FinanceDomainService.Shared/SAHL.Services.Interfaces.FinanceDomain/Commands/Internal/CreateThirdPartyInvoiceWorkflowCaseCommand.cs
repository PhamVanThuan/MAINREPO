using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class CreateThirdPartyInvoiceWorkflowCaseCommand : ServiceCommand, IFinanceDomainInternalCommand, IRequiresAccount, IRequiresThirdPartyInvoice
    {
        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        [Required]
        public int AccountKey { get; protected set; }

        [Required]
        public int ThirdPartyTypeKey { get; protected set; }

        public string ReceivedFrom { get; protected set; }

        public CreateThirdPartyInvoiceWorkflowCaseCommand(int thirdPartyInvoiceKey, int accountKey, int thirdPartyTypeKey, string receivedFrom)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.AccountKey = accountKey;
            this.ThirdPartyTypeKey = thirdPartyTypeKey;
            this.ReceivedFrom = receivedFrom;
        }
    }
}