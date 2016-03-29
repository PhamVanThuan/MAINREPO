using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices
{
    public class ThirdPartyInvoicesChildDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<ThirdPartyInvoicesChildTileConfiguration>
    {
        public ThirdPartyInvoicesChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}