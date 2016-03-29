using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.MortgageLoanAccount.Default
{
    public class MortgageLoanAccountMinorTileContentProvider : AbstractSqlTileContentProvider<MortgageLoanAccountMinorTileModel>
    {
        public override string GetStatement(BusinessModel.BusinessKey businessKey)
        {
            return string.Format(@"SELECT {0} as AccountNumber,
                                    [2am].dbo.fGetFormattedAddress(prop.AddressKey) as PropertyAddress,
                                    sum(ISNULL(lb.InitialBalance,0)) as LoanAgreementAmount,
                                    sum(cur_bal.Amount) as LoanCurrentBalance,
                                    sum(arr_bal.Amount) as LoanArrearBalance,
                                    fsb.DebitOrderDay,
                                    sum(fs.Payment) as InstallmentAmount,
                                    lb.RemainingInstalments,
                                    isnull(fa.MonthsInArrears,0) as  MonthsInArrears
                                        FROM [2am].dbo.Property prop (NOLOCK)
                                        JOIN [2am].fin.MortgageLoan ml (NOLOCK) on ml.PropertyKey = prop.PropertyKey
                                        JOIN [2am].dbo.FinancialService fs (NOLOCK) on ml.FinancialServiceKey = fs.FinancialServiceKey
                                        JOIN [2am].fin.LoanBalance lb (NOLOCK) on lb.FinancialServiceKey = fs.FinancialServiceKey and fs.FinancialServiceTypeKey = 1
                                        JOIN [2am].fin.Balance cur_bal (NOLOCK) on cur_bal.FinancialServiceKey = fs.FinancialServiceKey and cur_bal.BalanceTypeKey = 1
                                        JOIN [2am].dbo.FinancialService afs (NOLOCK) on afs.ParentFinancialServiceKey = fs.FinancialServiceKey
                                        JOIN [2am].fin.Balance arr_bal (NOLOCK) on arr_bal.FinancialServiceKey = afs.FinancialServiceKey and arr_bal.BalanceTypeKey = 4
                                        LEFT OUTER JOIN [2am].dbo.FinancialServiceBankAccount fsb (NOLOCK) on fs.financialservicekey = fsb.financialservicekey and fsb.GeneralStatusKey = 1
                                        LEFT OUTER JOIN dw.DWWAREHOUSEPRE.Securitisation.FactAccountAttribute fa on fa.AccountKey = fs.AccountKey
                                    WHERE fs.AccountKey = {0}
                                    GROUP BY fs.AccountKey,prop.AddressKey,fsb.DebitOrderDay,lb.RemainingInstalments,fa.MonthsInArrears", businessKey.Key);
        }
    }
}