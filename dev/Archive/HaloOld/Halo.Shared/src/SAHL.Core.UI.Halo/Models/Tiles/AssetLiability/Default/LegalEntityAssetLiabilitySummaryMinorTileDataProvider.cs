using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
namespace SAHL.Core.UI.Halo.Tiles.AssetLiability.Default
{
    public class LegalEntityAssetLiabilitySummaryMinorTileDataProvider : ITileDataProvider<LegalEntityAssetLiabilitySummaryMinorTileModel>
    {
        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}