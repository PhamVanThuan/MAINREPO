using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.ResidentialAddress
{
    public class LegalEntityResidentialAddressDrillDownTileConfiguration : MajorTileConfiguration<LegalEntityResidentialAddressMajorTileModel>,
        IDrillDownTileConfiguration<LegalEntityResidentialAddressMinorTileConfiguration>
    {
        public LegalEntityResidentialAddressDrillDownTileConfiguration()
            :base("LegalEntityResidentialAddressTileAccess", "Residential Address")
        {

        }
    }
}
