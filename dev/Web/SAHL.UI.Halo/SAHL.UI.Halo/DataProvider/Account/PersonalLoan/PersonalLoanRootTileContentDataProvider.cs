using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.PersonalLoan
{
    public class PersonalLoanRootTileContentDataProvider : HaloTileBaseContentDataProvider<PersonalLoanRootModel>
    {
        public PersonalLoanRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select 
                        ac.AccountKey as AccountNumber,
                        os.OriginationSourceKey,
                        IsInArrears = case
                            when Arrear.ArrearBalance > 0 then 1
                            else 0
                        end,
                        IsInAdvance = case
                            when Arrear.ArrearBalance < 0 then 1
                            else 0
                        end,
                        isnull(fa.FinancialAdjustmentStatusKey,0) as IsNonPerforming,
                        sa.Description as AccountStatus,
                        hic.TotalConfirmedIncome,
                        lb.InitialBalance as LoanAmount,
                        fsba.DebitOrderDay,
                        ac.OpenDate,
                        ac.CloseDate,
                        lb.MTDInterest as MonthToDateInterest,
                        b.Amount CurrentBalance,
                        Arrear.ArrearBalance,
                        lb.RemainingInstalments as RemainingTerm,
                        lb.InterestRate,
                        fs.Payment as MonthlyInstalment,
                        fee.Amount as MonthlyServiceFee,
                        clpfs.Payment as CreditLifePremium,
                        1 as OriginationSource,
                        (isnull(fs.Payment,0) + isnull(fee.Amount,0) + isnull(clpfs.Payment,0)) as TotalInstalment
                    from 
                        [2am].[dbo].[Account] ac
                    join 
                        [2am].[dbo].[OriginationSourceProduct] osp on osp.OriginationSourceProductKey=ac.OriginationSourceProductKey
                    join
                        [2am].[dbo].[OriginationSource] os on os.OriginationSourceKey=osp.OriginationSourceKey
                    join 
                        [2am].[dbo].[AccountStatus] sa on sa.AccountStatusKey=ac.[AccountStatusKey]
                    join
                        [2am].[dbo].[FinancialService] fs on fs.AccountKey=ac.AccountKey
                    join 
                        [2am].[dbo].[FinancialServiceType] fst on fst.FinancialServiceTypeKey=fs.FinancialServiceTypeKey and fst.BalanceTypeKey=1
                    join 
                        [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
                    join 
                        [2am].[fin].[LoanBalance] lb on lb.FinancialServiceKey=fs.FinancialServiceKey
                    join
                        [2am].[dbo].[FinancialServiceBankAccount] fsba on fsba.FinancialServiceKey=fs.FinancialServiceKey and fsba.GeneralStatusKey=1
                    join
                        (select
                            fs.accountkey,
                            b.Amount as ArrearBalance
                        from 
                            [2am].[dbo].[FinancialService] fs
                        join 
                            [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=4
                        where 
                            fs.AccountKey={0}
                            and fs.FinancialServiceTypeKey=8
                        ) Arrear on Arrear.Accountkey=ac.accountkey
                    left join
                        [2am].[fin].[FinancialAdjustment] fa on fa.FinancialServiceKey=fs.FinancialServiceKey 
                                                             and fa.FinancialAdjustmentStatusKey=1 and fa.FinancialAdjustmentSourceKey=2 -- non-performing
                    left join  
                        [2am].[fin].[Fee] fee on fs.FinancialServiceKey=fee.FinancialServiceKey
                    left join
                        (
                            select 
                                r.accountkey, sum(ConfirmedIncome) as TotalConfirmedIncome
                            from 
                                [2am].dbo.role r
                            left join
                                [2am].dbo.employment e on e.LegalEntityKey=r.LegalEntityKey
                            where
                                r.accountkey={0}
                            and 
                                r.generalstatuskey=1
                            and
                                employmentenddate is null 
                            group by
                            r.accountkey
                        ) hic on hic.accountkey=ac.accountkey
                    left join 
                        [2am].[dbo].[Account] clpa on clpa.ParentAccountKey=ac.AccountKey
                    left join
                        [2am].[dbo].[FinancialService] clpfs on clpfs.AccountKey=clpa.AccountKey and clpfs.FinancialServiceTypeKey=11
                    where 
                        ac.AccountKey={0}", businessContext.BusinessKey.Key);
        }
    }
}
