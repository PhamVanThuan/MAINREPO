using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.BusinessModel;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.AccountDetail
{
    public class MortgageLoanDetailsDrillDownTileConfiguration : MajorTileConfiguration<MortgageLoanAccountDetailsMajorTileModel>, 
                                                                 IDrillDownTileConfiguration<LegalEntityMortgageLoanAccountMinorTileConfiguration>
    {
        public MortgageLoanDetailsDrillDownTileConfiguration()
            : base("MortgageLoanDetailsTileAccess", "Details")
        {

        }
    }
}
