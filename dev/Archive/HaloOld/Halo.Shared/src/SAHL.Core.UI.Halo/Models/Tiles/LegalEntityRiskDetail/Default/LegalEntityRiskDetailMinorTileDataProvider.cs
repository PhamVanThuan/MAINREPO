using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System.Collections.Generic;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntityRiskDetail.Default
{
    public class LegalEntityRiskDetailMinorTileDataProvider : ITileDataProvider<LegalEntityRiskDetailMinorTileModel>
    {
        public LegalEntityRiskDetailMinorTileDataProvider()
            : base()
        {
        }

        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}