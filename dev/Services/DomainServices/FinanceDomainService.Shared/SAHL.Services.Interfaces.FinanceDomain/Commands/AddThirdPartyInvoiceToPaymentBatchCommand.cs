using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class AddThirdPartyInvoiceToPaymentBatchCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice, IRequiresCATSPaymentBatch
    {
        public int CATSPaymentBatchKey { get; protected set; }

        public int ThirdPartyInvoiceKey { get; protected set; } 

        public AddThirdPartyInvoiceToPaymentBatchCommand(int catsPaymentBatchKey, int thirdPartyInvoiceKey)
        {
            this.CATSPaymentBatchKey = catsPaymentBatchKey;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}