using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.RelatedLegalEntities.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard
{
    public class LegalEntityRelatedLegalEntitiesMiniTileConfiguration : MiniTileConfiguration<RelatedLegalEntitiesMinorTileModel>, IParentedTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public LegalEntityRelatedLegalEntitiesMiniTileConfiguration()
            : base("LegalEntityRelatedLegalEntitiesMiniTileAccess", 0)
        {
        }
    }
}