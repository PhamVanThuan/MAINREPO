using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Common.ArrearTransactions;
using SAHL.UI.Halo.Shared.Configuration.TileContent;

namespace SAHL.UI.Halo.DataProvider.Common.ArrearTransactions
{
    public class ArrearTransactionsChildTileContentDataProvider : HaloTileBaseContentMultipleDataProvider<ArrearTransactionChildModel>
    {
        public ArrearTransactionsChildTileContentDataProvider(IDbFactory dbFactory)
        : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT top 3
                                    AT.InsertDate as InsertDate,
                                    TT.[Description] AS TransactionTypeDescription,
                                    AT.Amount as Amount
                                FROM [2AM].[fin].ArrearTransaction AT (NOLOCK)
                                INNER JOIN [2AM].[fin].[TransactionType] TT (NOLOCK) ON AT.TransactionTypeKey = TT.TransactionTypeKey
                                INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                                INNER JOIN [2AM].[fin].[TransactionGroup] TG (NOLOCK) ON TTG.TransactionGroupKey = TG.TransactionGroupKey
                                INNER JOIN [2AM].[dbo].[TransactionTypeUI] TTUI (NOLOCK) ON TT.TransactionTypeKey = TTUI.TransactionTypeKey
                                INNER JOIN [2am].[fin].[ArrearTransactionAccountBalance] ATBAL (NOLOCK) ON AT.ArrearTransactionKey = ATBAL.ArrearTransactionKey 
                                INNER JOIN [2AM].[dbo].[FinancialService] FS (NOLOCK) ON AT.FinancialServiceKey = FS.FinancialServiceKey
                                INNER JOIN [2AM].[fin].Balance BAL (NOLOCK) ON FS.FinancialServiceKey = BAL.FinancialServiceKey
                                AND BAL.BalanceTypeKey = 4
                                INNER JOIN [2AM].[dbo].[FinancialService] FS_Parent (NOLOCK) ON FS.ParentFinancialServiceKey = FS_Parent.FinancialServiceKey
                                INNER JOIN [2AM].[dbo].[FinancialServiceType] FST_Parent (NOLOCK) ON FS_Parent.FinancialServiceTypeKey = FST_Parent.FinancialServiceTypeKey
                                WHERE FS.AccountKey = {0} AND (TTG.TransactionGroupKey = 1 OR TTG.TransactionGroupKey = 2) 
                                order by ttg.TransactionGroupKey asc, at.ArrearTransactionKey desc", businessContext.BusinessKey.Key);
}
    }
}