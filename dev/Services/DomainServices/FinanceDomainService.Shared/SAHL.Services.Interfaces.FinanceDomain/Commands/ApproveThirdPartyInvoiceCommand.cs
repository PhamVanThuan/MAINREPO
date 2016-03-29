using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands
{
    public class ApproveThirdPartyInvoiceCommand : ServiceCommand, IFinanceDomainCommand, IRequiresThirdPartyInvoice
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ApproveThirdPartyInvoiceCommand(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}