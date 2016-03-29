using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries
{
    public class ThirdPartyInvoiceQueriesChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                            IHaloTileModel<ThirdPartyInvoiceQueryChildModel>
    {
        public ThirdPartyInvoiceQueriesChildTileConfiguration()
            : base("Invoice Queries", "ThirdPartyInvoiceQueries", 3, startRow: 1, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}
