using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntity.Default
{
    public class LegalEntityMajorTileDataProvider : ITileDataProvider<LegalEntityMajorTileModel>
    {
        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}