using SAHL.Core.UI.Providers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Tiles.PersonalLoanAccount.Default
{
    public class PersonalLoanAccountDetailsMajorTileContentProvider : AbstractSqlTileContentProvider<PersonalLoanAccountDetailsMajorTileModel>
    {
        public override string GetStatement(BusinessModel.BusinessKey businessKey)
        {
            return string.Format("select {0} as AccountNumber", businessKey.Key);
        }
    }
}
