using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class EscalateThirdPartyInvoiceForApprovalCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice
    {
        public EscalateThirdPartyInvoiceForApprovalCommand(int thirdPartyInvoiceKey, int uosKeyForEscalatedUser)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.UOSKeyForEscalatedUser = uosKeyForEscalatedUser;
        }

        [Required]
        public int ThirdPartyInvoiceKey { get; protected set; }

        public int UOSKeyForEscalatedUser { get; protected set; }
    }
}