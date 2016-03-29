using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.FinancialProfile;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.FinancialProfile
{
    public class FinancialProfileChildTileContentDataProvider : HaloTileBaseContentDataProvider<FinancialProfileChildModel>
    {
        public FinancialProfileChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
                AgeOfOldestConfirmationOfIncome = dbo.fCalculateAge(OldestConfirmationOfIncomeDate, getdate()),
                isnull(TotalConfirmedIncome,0) as TotalConfirmedIncome,
                isnull(CurrentTotalExpensesProvidedOnMostRecentSAHLLoanApplication,0) as CurrentTotalExpensesProvidedOnMostRecentSAHLLoanApplication,
                isnull(CurrentTotalMonthlyPaymentExposureToSAHL,0) as CurrentTotalMonthlyPaymentExposureToSAHL,
                isnull(CurrentTotalOutstandingBalanceLoanExposureToSAHL,0) as CurrentTotalOutstandingBalanceLoanExposureToSAHL,
                isnull(CurrentTotalArrearBalanceExposureToSAHL,0) as CurrentTotalArrearBalanceExposureToSAHL
            from
                    (
                        select LegalEntityKey, min(ConfirmedDate) as OldestConfirmationOfIncomeDate,  sum(SubTotalConfirmedIncome) TotalConfirmedIncome
                        from   (select
                                    e.LegalEntityKey,
                                    e.ConfirmedDate,
                                    sum(e.ConfirmedIncome) SubTotalConfirmedIncome
                                from
                                    [2am].dbo.employment e
                                where
                                e.LegalEntityKey={0}
                                and e.EmploymentStartDate <= getdate()
                                and e.EmploymentEndDate is null
                                group by e.LegalEntityKey, e.ConfirmedDate
                                ) income
                        group by
                            LegalEntityKey
                    ) ConfirmedIncome
            left join
                    (
                    select
                        lea.LegalEntityKey,
                        sum(amount) CurrentTotalExpensesProvidedOnMostRecentSAHLLoanApplication
                    from
                        [2am].[dbo].LegalEntityAffordability lea
                    join
                        [2am].[dbo].AffordabilityType aft on aft.AffordabilityTypeKey=lea.AffordabilityTypeKey and aft.AffordabilityTypeGroupKey != 1
                    where
                        lea.LegalEntityKey={0}
                        and lea.offerkey = (select top 1 offerkey from [2am].[dbo].LegalEntityAffordability where LegalEntityKey={0} order by offerkey)
                    group by
                        lea.LegalEntityKey
                    ) Expenses on Expenses.LegalEntityKey=ConfirmedIncome.LegalEntityKey
            left join
                    (select
                        r.LegalEntityKey,
                        sum(fs.Payment) CurrentTotalMonthlyPaymentExposureToSAHL
                    from
                        [2am].[dbo].[Role] r
                    join
                        [2am].[dbo].[Account] a on a.AccountKey=r.AccountKey
                    join
                        [2am].[dbo].[FinancialService] fs on fs.AccountKey=a.AccountKey
                    join
                        [2am].[dbo].[FinancialServiceType] fst on fst.FinancialServiceTypeKey=fs.FinancialServiceTypeKey and fst.BalanceTypeKey=1
                    join
                        [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
                    where
                        r.LegalEntityKey={0}
                        and r.GeneralStatusKey=1 -- Active role
                        and fs.AccountStatusKey=1 -- Not Closed
                        and fs.CloseDate is null -- Not closed
                    group by r.legalentitykey
                    ) Payment on Payment.LegalEntityKey=Expenses.LegalEntityKey
            left join
                    (select
                        r.LegalEntityKey,
                        (sum(b.Amount) + sum(isnull(fee.Amount, 0.0))) as CurrentTotalOutstandingBalanceLoanExposureToSAHL
                    from
                        [2am].[dbo].[Role] r
                    join
                        [2am].[dbo].[Account] a on a.AccountKey=r.AccountKey
                    join
                        [2am].[dbo].[FinancialService] fs on fs.AccountKey=a.AccountKey and FinancialServiceTypeKey in (1,2)
                    join
                        [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
                    left join
                        [2am].[fin].[Fee] fee on fee.FinancialServiceKey=fs.FinancialServiceKey
                    where
                        r.LegalEntityKey={0}
                        and r.GeneralStatusKey=1 -- Active role
                        and fs.AccountStatusKey=1 -- Not Closed
                        and fs.CloseDate is null -- Not closed
                    group by r.legalentitykey
                    ) ExposureBalance on ExposureBalance.LegalEntityKey=Payment.LegalEntityKey
            left join (select
                        r.LegalEntityKey,
                        sum(b.Amount) CurrentTotalArrearBalanceExposureToSAHL
                    from
                        [2am].[dbo].[Role] r
                    join
                        [2am].[dbo].[Account] a on a.AccountKey=r.AccountKey
                    join
                        [2am].[dbo].[FinancialService] fs on fs.AccountKey=a.AccountKey and fs.FinancialServiceTypeKey=8 -- Arrear
                    join
                        [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=4
                    where
                        r.LegalEntityKey={0}
                        and r.GeneralStatusKey=1 -- Active role
                        and fs.AccountStatusKey=1 -- Not Closed
                        and fs.CloseDate is null -- Not closed
                    group by r.legalentitykey
                    ) ArrearBalance on ArrearBalance.LegalEntityKey=Payment.LegalEntityKey", businessContext.BusinessKey.Key);
        }
    }
}