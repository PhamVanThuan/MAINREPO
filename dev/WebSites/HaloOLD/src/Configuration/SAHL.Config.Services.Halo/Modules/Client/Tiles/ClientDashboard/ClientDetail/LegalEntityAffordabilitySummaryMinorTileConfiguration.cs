using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.Affordability.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityAffordabilitySummaryMinorTileConfiguration : MinorTileConfiguration<LegalEntityAffordabilitySummaryMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityAffordabilitySummaryMinorTileConfiguration()
            : base("", 8)
        {
        }
    }
}