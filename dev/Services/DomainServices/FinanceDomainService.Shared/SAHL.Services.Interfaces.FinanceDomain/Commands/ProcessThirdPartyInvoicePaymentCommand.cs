using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class ProcessThirdPartyInvoicePaymentCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice, IThirdPartyInvoiceRuleModel
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ProcessThirdPartyInvoicePaymentCommand(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
