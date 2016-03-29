using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.PowerOfAttorney.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard
{
    public class LegalEntityPowerOfAttorneyMinorTileConfiguration : MinorTileConfiguration<LegalEntityPowerOfAttorneyMinorTileModel>, IParentedTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public LegalEntityPowerOfAttorneyMinorTileConfiguration()
            : base("", 0)
        {
        }
    }
}