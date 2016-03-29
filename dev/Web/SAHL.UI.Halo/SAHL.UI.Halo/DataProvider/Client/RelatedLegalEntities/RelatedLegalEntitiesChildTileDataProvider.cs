using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.RelatedLegalEntities;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Clients.RelatedLegalEntities
{
    public class RelatedLegalEntitiesChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<RelatedLegalEntitiesChildTileConfiguration>
    {
        public RelatedLegalEntitiesChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}