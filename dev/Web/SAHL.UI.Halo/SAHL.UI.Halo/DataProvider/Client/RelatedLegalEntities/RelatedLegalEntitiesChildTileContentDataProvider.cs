using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Common.RelatedLegalEntities;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.RelatedLegalEntities
{
    public class RelatedLegalEntitiesChildTileContentDataProvider : HaloTileBaseContentDataProvider<RelatedLegalEntitiesChildModel>
    {
        public RelatedLegalEntitiesChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return "";
        }
    }
}