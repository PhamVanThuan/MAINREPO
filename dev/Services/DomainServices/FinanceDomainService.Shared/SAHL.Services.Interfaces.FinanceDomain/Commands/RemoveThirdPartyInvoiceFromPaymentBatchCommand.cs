using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class RemoveThirdPartyInvoiceFromPaymentBatchCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice, IRequiresCATSPaymentBatch
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public int CATSPaymentBatchKey { get; protected set; }

        public RemoveThirdPartyInvoiceFromPaymentBatchCommand(int catsPaymentBatchKey, int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            CATSPaymentBatchKey = catsPaymentBatchKey;
        }
    }
}
