using SAHL.UI.Halo.Models.Common.ArrearTransactions;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.ArrearTransactions
{
    public class ArrearTransactionsChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<MortgageLoanRootTileConfiguration>,
                                                            IHaloTileModel<ArrearTransactionChildModel>
    {
        public ArrearTransactionsChildTileConfiguration()
            : base("Arrear Transactions", "ArrearTransactions", 3, noOfRows: 2, noOfColumns: 4)
        {
        }
    }
}