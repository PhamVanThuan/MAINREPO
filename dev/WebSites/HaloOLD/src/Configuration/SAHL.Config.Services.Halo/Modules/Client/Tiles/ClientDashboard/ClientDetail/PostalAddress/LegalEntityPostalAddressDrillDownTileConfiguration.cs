using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.PostalAddress
{
    public class LegalEntityPostalAddressDrillDownTileConfiguration : MajorTileConfiguration<LegalEntityPostalAddressMajorTileModel>,
        IDrillDownTileConfiguration<LegalEntityPostalAddressMinorTileConfiguration>
    {
        public LegalEntityPostalAddressDrillDownTileConfiguration()
            : base("LegalEntityPostalAddressTileAccess", "Postal Address")
        {
        }
    }
}