using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default;
namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class LegalEntityAddressMinorTileConfiguration : MinorTileConfiguration<LegalEntityAddressMinorTileModel>, IParentedTileConfiguration<LegalEntityDetailsDrillDownTileConfiguration>
    {
        public LegalEntityAddressMinorTileConfiguration()
            : base("LegalEntityAddressTileAccess", 0)
        {
        }
    }
}