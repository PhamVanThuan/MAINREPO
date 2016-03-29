using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailRootTileDataProvider : HaloTileBaseEditorDataProvider<ThirdPartyInvoiceDetailRootModel>
    {
        public ThirdPartyInvoiceDetailRootTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}