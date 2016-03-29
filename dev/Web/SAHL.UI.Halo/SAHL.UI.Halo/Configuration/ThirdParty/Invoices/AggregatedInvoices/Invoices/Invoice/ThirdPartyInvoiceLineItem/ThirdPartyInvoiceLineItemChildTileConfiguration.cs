using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceLineItem
{
    public class ThirdPartyInvoiceLineItemChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                            IHaloTileModel<ThirdPartyInvoiceLineItemModel>
    {
        public ThirdPartyInvoiceLineItemChildTileConfiguration()
            : base("Invoices", "ThirdPartyInvoices", 1, noOfRows: 4, noOfColumns: 4)
        {
        }
    }
}
