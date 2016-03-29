using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityResidentialAddressMinorTileConfiguration : MinorTileConfiguration<LegalEntityResidentialAddressMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityResidentialAddressMinorTileConfiguration()
            : base("", 0)
        {
        }
    }
}