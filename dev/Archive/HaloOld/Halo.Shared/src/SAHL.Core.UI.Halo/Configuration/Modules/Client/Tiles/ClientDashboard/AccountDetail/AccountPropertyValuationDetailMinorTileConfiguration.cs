﻿using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.AccountPropertyValuationDetail.Default;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.AccountDetail
{
    public class AccountPropertyValuationDetailMinorTileConfiguration : MinorTileConfiguration<AccountPropertyValuationDetailMinorTileModel>, IParentedTileConfiguration<MortgageLoanAccountDetailsDrillDownTileConfiguration>
    {
        public AccountPropertyValuationDetailMinorTileConfiguration()
            : base("AccountPropertyValuationDetailMinorTileAccess", 1)
        {
        }
    }
}