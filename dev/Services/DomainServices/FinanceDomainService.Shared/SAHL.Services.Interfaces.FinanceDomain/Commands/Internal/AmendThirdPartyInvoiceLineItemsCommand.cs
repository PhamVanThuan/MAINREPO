using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class AmendThirdPartyInvoiceLineItemsCommand : ServiceCommand, IFinanceDomainCommand
    {
        public IEnumerable<InvoiceLineItemModel> lineItemsToUpdate { get; protected set; }

        public AmendThirdPartyInvoiceLineItemsCommand(IEnumerable<InvoiceLineItemModel> lineItemsToUpdate)
        {
            this.lineItemsToUpdate = lineItemsToUpdate;
        }
    }
}