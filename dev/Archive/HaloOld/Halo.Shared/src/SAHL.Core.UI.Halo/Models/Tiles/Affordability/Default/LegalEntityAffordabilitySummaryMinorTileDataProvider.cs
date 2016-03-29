using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
namespace SAHL.Core.UI.Halo.Tiles.Affordability.Default
{
    public class LegalEntityAffordabilitySummaryMinorTileDataProvider : ITileDataProvider<LegalEntityAffordabilitySummaryMinorTileModel>
    {
        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}