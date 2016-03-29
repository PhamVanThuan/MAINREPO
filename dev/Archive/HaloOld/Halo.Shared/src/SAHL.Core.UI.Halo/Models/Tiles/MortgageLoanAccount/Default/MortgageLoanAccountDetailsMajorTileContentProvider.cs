using SAHL.Core.UI.Providers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default
{
    public class MortgageLoanAccountDetailsMajorTileContentProvider : AbstractSqlTileContentProvider<MortgageLoanAccountDetailsMajorTileModel>
    {
        public override string GetStatement(BusinessModel.BusinessKey businessKey)
        {
            return string.Format("select {0} as AccountNumber", businessKey.Key);
        }
    }
}
