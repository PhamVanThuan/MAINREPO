using SAHL.DomainServiceChecks.Checks;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresThirdPartyInvoice
{
    public class CommandToCheck : IRequiresThirdPartyInvoice
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public Guid Id  { get; protected set; }

        public CommandToCheck(Guid Id, int thirdPartyInvoiceKey)
        {
            this.Id = Id;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}