using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.AssetLiability.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityAssetLiabilitySummaryMinorTileConfiguration : MinorTileConfiguration<LegalEntityAssetLiabilitySummaryMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityAssetLiabilitySummaryMinorTileConfiguration()
            : base("LegalEntityAssetLiabilitySummaryMinorTileAccess", 7)
        {
        }
    }
}