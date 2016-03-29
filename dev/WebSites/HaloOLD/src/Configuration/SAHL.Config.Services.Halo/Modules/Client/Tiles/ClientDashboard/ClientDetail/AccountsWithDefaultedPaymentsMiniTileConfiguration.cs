using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.AccountsWithDefaultedPayments.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail
{
    public class AccountsWithDefaultedPaymentsMiniTileConfiguration : MiniTileConfiguration<AccountsWithDefaultedPaymentsMinorTileModel>, IParentedTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public AccountsWithDefaultedPaymentsMiniTileConfiguration()
            :base("",0)
        {

        }
    }
}
