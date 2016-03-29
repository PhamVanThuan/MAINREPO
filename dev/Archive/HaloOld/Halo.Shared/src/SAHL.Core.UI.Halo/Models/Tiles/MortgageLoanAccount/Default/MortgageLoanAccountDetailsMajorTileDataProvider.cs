using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default
{
    public class MortgageLoanAccountDetailsMajorTileDataProvider : ITileDataProvider<MortgageLoanAccountDetailsMajorTileModel>
    {
        public IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessModel.BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}
