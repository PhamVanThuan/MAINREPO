using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntityRiskDetail.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityRiskDetailMinorTileConfiguration : MinorTileConfiguration<LegalEntityRiskDetailMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityRiskDetailMinorTileConfiguration()
            : base("", 0)
        {
        }
    }
}