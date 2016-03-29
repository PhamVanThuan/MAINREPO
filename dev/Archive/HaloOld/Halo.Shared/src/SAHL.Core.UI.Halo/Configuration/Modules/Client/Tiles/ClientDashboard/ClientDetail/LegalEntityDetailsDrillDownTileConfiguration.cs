using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntity.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityDetailsDrillDownTileConfiguration : MajorTileConfiguration<LegalEntityDetailsMajorTileModel>, IDrillDownTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public LegalEntityDetailsDrillDownTileConfiguration()
            : base("LegalEntityDetailsTileAccess", "Details")
        {
        }
    }
}