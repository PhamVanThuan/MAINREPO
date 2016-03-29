using SAHL.UI.Halo.Configuration.Account.ArrearTransaction;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.ArrearTransactions
{
    public class ArrearTransactionsChildPageDrilldown : HaloTileActionDrilldownBase<ArrearTransactionsChildTileConfiguration, ArrearTransactionsRootTileConfiguration>,
                                                    IHaloTileActionDrilldown<ArrearTransactionsChildTileConfiguration>
    {
        public ArrearTransactionsChildPageDrilldown()
            : base("Arrear Transaction")
        {
        }
    }
}