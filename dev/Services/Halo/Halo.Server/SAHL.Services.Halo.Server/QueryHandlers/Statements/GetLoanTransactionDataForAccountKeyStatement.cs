using SAHL.Core.Data;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Halo.Server.QueryHandlers.Statements
{
    public class GetLoanTransactionDataForAccountKeyStatement : ISqlStatement<LoanTransactionDetailModel>
    {
        public int AccountKey { get; protected set; }

        public GetLoanTransactionDataForAccountKeyStatement(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public string GetStatement()
        {
            return string.Format(@"SELECT
						FT.FinancialTransactionKey,
						FT.InsertDate,
						FT.EffectiveDate,
						FT.InterestRate,
						FT.Amount,
						FT.Balance,
						FT.Reference,
						FTBAL.AccountBalance,
                        FT.UserID,
						TT.[Description] AS TransactionTypeDescription,
						TTUI.HTMLColour AS TransactionTypeHTMLColour,
						FST.[Description] AS FinancialService,
						TG.[Description] As TransactionGroup
						FROM [2AM].[fin].FinancialTransaction FT (NOLOCK)
						INNER JOIN [2AM].[fin].[TransactionType] TT (NOLOCK) ON FT.TransactionTypeKey = TT.TransactionTypeKey
						INNER JOIN [2AM].[fin].[TransactionTypeGroup] TTG (NOLOCK) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
						INNER JOIN [2AM].[fin].[TransactionGroup] TG (NOLOCK) ON TTG.TransactionGroupKey = TG.TransactionGroupKey
						INNER JOIN [2AM].[dbo].[TransactionTypeUI] TTUI (NOLOCK) ON TT.TransactionTypeKey = TTUI.TransactionTypeKey
						INNER JOIN [2am].[fin].[FinancialTransactionAccountBalance] FTBAL (NOLOCK) ON FT.FinancialTransactionKey = FTBAL.FinancialTransactionKey
						INNER JOIN [2AM].[dbo].[FinancialService] FS (NOLOCK) ON FT.FinancialServiceKey = FS.FinancialServiceKey
						INNER JOIN [2AM].[fin].Balance BAL (NOLOCK) ON FS.FinancialServiceKey = BAL.FinancialServiceKey
																	AND BAL.BalanceTypeKey <> 4
						INNER JOIN [2AM].[dbo].[FinancialServiceType] FST (NOLOCK) ON FS.FinancialServiceTypeKey = FST.FinancialServiceTypeKey
						WHERE FS.AccountKey = @AccountKey AND (TTG.TransactionGroupKey = 1 OR TTG.TransactionGroupKey = 2)
						order by ttg.TransactionGroupKey asc, ft.financialTransactionKey desc");
        }
    }
}