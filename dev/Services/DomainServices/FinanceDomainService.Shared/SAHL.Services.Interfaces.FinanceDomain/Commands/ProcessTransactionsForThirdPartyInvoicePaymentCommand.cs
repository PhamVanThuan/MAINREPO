using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class ProcessTransactionsForThirdPartyInvoicePaymentCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice, IThirdPartyInvoiceRuleModel
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ProcessTransactionsForThirdPartyInvoicePaymentCommand(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
