using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
