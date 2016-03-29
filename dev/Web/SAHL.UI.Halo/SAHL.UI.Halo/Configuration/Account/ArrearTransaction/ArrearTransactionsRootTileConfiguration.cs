﻿using SAHL.UI.Halo.Models.Common.ArrearTransactions;
using SAHL.UI.Halo.Pages;
using SAHL.UI.Halo.Pages.Common.Transactions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.ArrearTransaction
{
    public class ArrearTransactionsRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<ArrearTransactionDetailModel>,
                                                     IHaloTilePageState<ArrearTransactionsPageState>
    {
        public ArrearTransactionsRootTileConfiguration()
            : base("Arrear Transaction", "ArrearTransaction", 5, false)
        {
        }
    }
}
