using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.Payment.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.AccountDetail
{
    public class PersonalLoanPaymentMinorTileConfiguration : MinorTileConfiguration<MortgageLoanPaymentMinorTileModel>, IParentedTileConfiguration<PersonalLoanDetailsDrillDownTileConfiguration>
    {
        public PersonalLoanPaymentMinorTileConfiguration()
            : base("", 0)
        {

        }
    }
}
