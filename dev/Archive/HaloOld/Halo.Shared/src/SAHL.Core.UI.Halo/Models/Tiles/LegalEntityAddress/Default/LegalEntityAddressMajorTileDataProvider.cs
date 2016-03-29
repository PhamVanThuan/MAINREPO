using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System.Collections.Generic;
namespace SAHL.Core.UI.Halo.Tiles.LegalEntityAddress.Default
{
    public class LegalEntityAddressMajorTileDataProvider : ITileDataProvider<LegalEntityAddressMajorTileModel>
    {
        public LegalEntityAddressMajorTileDataProvider()
            : base()
        {
        }

        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}