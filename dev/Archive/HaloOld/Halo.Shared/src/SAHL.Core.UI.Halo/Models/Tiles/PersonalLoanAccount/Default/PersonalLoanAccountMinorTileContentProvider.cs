using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.PersonalLoanAccount.Default
{
    public class PersonalLoanAccountMinorTileContentProvider : AbstractSqlTileContentProvider<PersonalLoanAccountMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"select a.AccountKey as AccountNumber,
		                                    lb.InitialBalance as LoanAmount,
		                                    a.OpenDate as OpenDate,
		                                    bal.Amount as CurrentBalance,
		                                    arrearbal.Amount as ArrearBalance,
		                                    isnull(fa.MonthsInArrears,0) as MonthsInArrears,
		                                    fsba.DebitOrderDay as DebitOrderDay,
		                                    fs.Payment as InstallmentAmount,
		                                    lb.RemainingInstalments as RemainingTerm
                                    from [2AM].dbo.Account a
                                    join [2AM].dbo.FinancialService fs on fs.AccountKey = a.AccountKey and fs.FinancialServiceTypeKey = 10
                                    join [2AM].fin.Balance bal on bal.FinancialServiceKey = fs.FinancialServiceKey and bal.BalanceTypeKey = 1
                                    join [2AM].fin.LoanBalance lb on lb.FinancialServiceKey = fs.FinancialServiceKey
                                    join [2AM].dbo.FinancialService arrearfs on arrearfs.AccountKey = a.AccountKey and arrearfs.FinancialServiceTypeKey = 8
                                    join [2AM].fin.Balance arrearbal on arrearbal.FinancialServiceKey = arrearfs.FinancialServiceKey and arrearbal.BalanceTypeKey = 4
                                    join [2AM].dbo.FinancialServiceBankAccount fsba on fsba.FinancialServiceKey = fs.FinancialServiceKey AND fsba.GeneralStatusKey = 1
                                    left outer join dw.DWWAREHOUSEPRE.Securitisation.FactAccountAttribute fa on fa.AccountKey = a.AccountKey
                                    where a.AccountKey = {0}", businessKey.Key);
        }
    }
}