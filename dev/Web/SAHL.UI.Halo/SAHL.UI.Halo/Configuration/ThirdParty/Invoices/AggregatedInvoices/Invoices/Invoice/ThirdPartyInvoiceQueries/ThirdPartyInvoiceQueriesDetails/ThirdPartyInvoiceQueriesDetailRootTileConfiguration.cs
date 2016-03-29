using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Pages.Common.Invoices;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries.ThirdPartyInvoiceQueriesDetails
{
    public class ThirdPartyInvoiceQueriesDetailRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                     IHaloModuleTileConfiguration<TaskHomeConfiguration>,
                                                     IHaloTileModel<ThirdPartyInvoiceQueryRootModel>,
                                                     IHaloTilePageState<ThirdPartyCorrespondencePageState>
    {
        public ThirdPartyInvoiceQueriesDetailRootTileConfiguration()
            : base("Invoice Queries", "ThirdPartyInvoiceQueriesDetail", 5, false)
        {

        }
    }
}
