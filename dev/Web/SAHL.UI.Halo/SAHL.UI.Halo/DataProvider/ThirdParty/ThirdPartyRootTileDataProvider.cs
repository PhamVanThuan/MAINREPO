using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty
{
    public class ThirdPartyRootTileDataProvider : HaloTileBaseEditorDataProvider<ThirdPartyRootModel>
    {
        public ThirdPartyRootTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}