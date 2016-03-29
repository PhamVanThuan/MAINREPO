using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertInvoiceLineItemsCommand : ServiceCommand, IFrontEndTestCommand
    {
        public IEnumerable<InvoiceLineItemDataModel> InvoiceLineItems { get; protected set; }

        public InsertInvoiceLineItemsCommand(IEnumerable<InvoiceLineItemDataModel> invoiceLineItems)
        {
            this.InvoiceLineItems = invoiceLineItems;
        }
    }
}
