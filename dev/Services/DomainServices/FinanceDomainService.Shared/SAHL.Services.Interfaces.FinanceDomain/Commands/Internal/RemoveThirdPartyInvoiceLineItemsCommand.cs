using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class RemoveThirdPartyInvoiceLineItemsCommand : ServiceCommand, IFinanceDomainCommand
    {
        public IEnumerable<InvoiceLineItemDataModel> lineItemsToRemove { get; protected set; }

        public RemoveThirdPartyInvoiceLineItemsCommand(IEnumerable<InvoiceLineItemDataModel> lineItemsToRemove)
        {
            this.lineItemsToRemove = lineItemsToRemove;
        }
    }
}