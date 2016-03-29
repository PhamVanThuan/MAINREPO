using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Tiles.Payment.Default
{
    public class MortgageLoanPaymentMinorTileDataProvider : ITileDataProvider<MortgageLoanPaymentMinorTileModel>
    {
        public IEnumerable<BusinessModel.BusinessKey> GetTileInstanceKeys(BusinessKey businessKey)
        {
            return new BusinessKey[] { businessKey };
        }
    }
}
