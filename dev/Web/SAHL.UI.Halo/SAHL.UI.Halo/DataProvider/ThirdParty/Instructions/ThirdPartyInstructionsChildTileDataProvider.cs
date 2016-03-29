using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Instructions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Instructions
{
    public class ThirdPartyInstructionsChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<ThirdPartyInstructionsChildTileConfiguration>
    {
        public ThirdPartyInstructionsChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}