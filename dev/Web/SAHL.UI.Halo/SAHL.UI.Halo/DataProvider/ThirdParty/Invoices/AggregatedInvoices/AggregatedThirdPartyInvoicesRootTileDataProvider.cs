using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices
{
    public class AggregatedThirdPartyInvoicesRootTileDataProvider : HaloTileBaseEditorDataProvider<AggregatedThirdPartyInvoiceRootModel>
    {
        public AggregatedThirdPartyInvoicesRootTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}