using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.Employment.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityEmploymentDetailMinorTileConfiguration : MinorTileConfiguration<LegalEntityEmploymentDetailMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityEmploymentDetailMinorTileConfiguration()
            : base("LegalEntityEmploymentDetailMinorTileAccess", 6)
        {
        }
    }
}