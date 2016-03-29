using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class ThirdPartyInvoiceAmendedEvent : Event
    {
        public ThirdPartyInvoiceModel AmmendedThirdPartyInvoice { get; protected set; }
        public ThirdPartyInvoiceDataModel OriginalThirdPartyInvoice { get; protected set; }

        public IEnumerable<InvoiceLineItemModel> AddedInvoiceLineItems { get; protected set; }
        public IEnumerable<InvoiceLineItemModel> UpdatedInvoiceLineItems { get; protected set; }
        public IEnumerable<InvoiceLineItemDataModel> RemovedInvoiceLineItems { get; protected set; }

        public ThirdPartyInvoiceAmendedEvent(DateTime date,
            ThirdPartyInvoiceDataModel originalThirdPartyInvoice
            , ThirdPartyInvoiceModel ammendedThirdPartyInvoice
            , IEnumerable<InvoiceLineItemModel> addedInvoiceLineItems
            , IEnumerable<InvoiceLineItemModel> updatedInvoiceLineItems
            , IEnumerable<InvoiceLineItemDataModel> removedInvoiceLineItems)
            : base(date)
        {
            this.OriginalThirdPartyInvoice =originalThirdPartyInvoice;
            this.AmmendedThirdPartyInvoice = ammendedThirdPartyInvoice;
            this.AddedInvoiceLineItems = addedInvoiceLineItems;
            this.UpdatedInvoiceLineItems = updatedInvoiceLineItems;
            this.RemovedInvoiceLineItems = removedInvoiceLineItems;
        }
    }
}
