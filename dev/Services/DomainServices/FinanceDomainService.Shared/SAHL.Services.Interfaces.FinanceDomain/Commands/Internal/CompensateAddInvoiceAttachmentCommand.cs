using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class CompensateAddInvoiceAttachmentCommand : ServiceCommand, IFinanceDomainCommand
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public CompensateAddInvoiceAttachmentCommand(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}