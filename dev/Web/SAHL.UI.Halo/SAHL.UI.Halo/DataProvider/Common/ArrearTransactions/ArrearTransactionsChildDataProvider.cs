using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.ArrearTransactions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Common.ArrearTransactions
{
    public class ArrearTransactionsChildDataProvider : HaloTileBaseChildDataProvider,
                                         IHaloTileChildDataProvider<ArrearTransactionsChildTileConfiguration>
    {
        public ArrearTransactionsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}