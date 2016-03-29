using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.LegalEntity.Default;
using SAHL_Config.Website.Halo.Modules.Client;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles
{
    public class LegalEntityRootTileConfiguration : MajorTileConfiguration<LegalEntityMajorTileModel>,
        IRootTileConfiguration<ClientApplicationModule>
    {
        public LegalEntityRootTileConfiguration()
            : base("LegalEntityRootTileAccess", "Client")
        {
        }
    }
}