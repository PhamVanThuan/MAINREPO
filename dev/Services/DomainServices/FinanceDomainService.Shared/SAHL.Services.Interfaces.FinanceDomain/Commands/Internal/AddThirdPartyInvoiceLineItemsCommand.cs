using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Commands.Internal
{
    public class AddThirdPartyInvoiceLineItemsCommand : ServiceCommand, IFinanceDomainCommand
    {
        public IEnumerable<InvoiceLineItemModel> NewInvoiceLineItems { get; protected set; }

        public AddThirdPartyInvoiceLineItemsCommand(IEnumerable<InvoiceLineItemModel> newInvoiceLineItems)
        {
            this.NewInvoiceLineItems = newInvoiceLineItems;
        }
    }
}
