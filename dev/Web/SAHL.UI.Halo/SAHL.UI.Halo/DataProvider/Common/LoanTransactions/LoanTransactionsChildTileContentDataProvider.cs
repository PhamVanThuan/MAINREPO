using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Common.LoanTransaction;
using SAHL.UI.Halo.Shared.Configuration.TileContent;

namespace SAHL.UI.Halo.DataProvider.Common.LoanTransactions
{
    public class LoanTransactionsChildTileContentDataProvider : HaloTileBaseContentMultipleDataProvider<LoanTransactionTileModel>
    {
        public LoanTransactionsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT  top 3 
FT.InsertDate as InsertDate,
TT.[Description] AS TransactionTypeDescription,
FT.Amount as Amount
FROM [2AM].[fin].FinancialTransaction FT (NOLOCK)
INNER JOIN [2AM].[fin].[TransactionType] TT (NOLOCK) ON FT.TransactionTypeKey = TT.TransactionTypeKey
INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
INNER JOIN [2AM].[fin].[TransactionGroup] TG (NOLOCK) ON TTG.TransactionGroupKey = TG.TransactionGroupKey
INNER JOIN [2am].[fin].[FinancialTransactionAccountBalance] FTBAL (NOLOCK) ON FT.FinancialTransactionKey = FTBAL.FinancialTransactionKey 
INNER JOIN [2AM].[dbo].[FinancialService] FS (NOLOCK) ON FT.FinancialServiceKey = FS.FinancialServiceKey
INNER JOIN [2AM].[fin].Balance BAL (NOLOCK) ON FS.FinancialServiceKey = BAL.FinancialServiceKey
                                            AND BAL.BalanceTypeKey <> 4
INNER JOIN [2AM].[dbo].[FinancialServiceType] FST (NOLOCK) ON FS.FinancialServiceTypeKey = FST.FinancialServiceTypeKey
WHERE FS.AccountKey = {0} AND (TTG.TransactionGroupKey = 1 OR TTG.TransactionGroupKey = 2) 
order by ttg.TransactionGroupKey asc, ft.financialTransactionKey desc", businessContext.BusinessKey.Key);
        }
    }
}
