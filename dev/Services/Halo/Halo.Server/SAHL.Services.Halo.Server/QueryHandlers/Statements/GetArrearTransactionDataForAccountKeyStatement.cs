using SAHL.Core.Data;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Halo.Server.QueryHandlers.Statements
{
    public class GetArrearTransactionDataForAccountKeyStatement : ISqlStatement<ArrearTransactionDetailModel>
    {
        public int AccountKey { get; protected set; }

        public GetArrearTransactionDataForAccountKeyStatement(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public string GetStatement()
        {
            return string.Format(@"SELECT  
                                    AT.ArrearTransactionKey,
                                    AT.InsertDate,
                                    AT.EffectiveDate,
                                    AT.Amount,
                                    AT.Balance,
                                    AT.Reference,
                                    AT.Userid AS UserID,                                    
                                    ATBAL.AccountBalance,
                                    TT.[Description] AS TransactionTypeDescription,
                                    FST_Parent.[Description] AS FinancialService,
                                    TG.[Description] As TransactionGroup
                                    FROM[2AM].[fin].ArrearTransaction AT(NOLOCK)
                                    INNER JOIN[2AM].[fin].[TransactionType] TT(NOLOCK) ON AT.TransactionTypeKey = TT.TransactionTypeKey
                                    INNER JOIN[2AM].[fin].[TransactionTypeGroup] TTG(NOLOCK) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                                    INNER JOIN[2AM].[fin].[TransactionGroup] TG(NOLOCK) ON TTG.TransactionGroupKey = TG.TransactionGroupKey
                                    INNER JOIN[2AM].[dbo].[TransactionTypeUI] TTUI(NOLOCK) ON TT.TransactionTypeKey = TTUI.TransactionTypeKey
                                    INNER JOIN[2am].[fin].[ArrearTransactionAccountBalance] ATBAL(NOLOCK) ON AT.ArrearTransactionKey = ATBAL.ArrearTransactionKey
                                    INNER JOIN[2AM].[dbo].[FinancialService] FS(NOLOCK) ON AT.FinancialServiceKey = FS.FinancialServiceKey
                                    INNER JOIN[2AM].[fin].Balance BAL(NOLOCK) ON FS.FinancialServiceKey = BAL.FinancialServiceKey
                                    AND BAL.BalanceTypeKey = 4
                                    INNER JOIN[2AM].[dbo].[FinancialService] FS_Parent(NOLOCK) ON FS.ParentFinancialServiceKey = FS_Parent.FinancialServiceKey
                                    INNER JOIN[2AM].[dbo].[FinancialServiceType] FST_Parent(NOLOCK) ON FS_Parent.FinancialServiceTypeKey = FST_Parent.FinancialServiceTypeKey
                                    WHERE FS.AccountKey = @AccountKey AND(TTG.TransactionGroupKey = 1 OR TTG.TransactionGroupKey = 2)
                                    order by ttg.TransactionGroupKey asc, at.ArrearTransactionKey desc");
        }
    }
}
