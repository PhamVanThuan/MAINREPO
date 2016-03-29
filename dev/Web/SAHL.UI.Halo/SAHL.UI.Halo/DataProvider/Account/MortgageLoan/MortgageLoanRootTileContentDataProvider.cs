using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan
{
    public class MortgageLoanRootTileContentDataProvider : HaloTileBaseContentDataProvider<MortgageLoanRootModel>
    {
        public MortgageLoanRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select distinct
    AccountNumber = {0},
    OriginationSourceKey = case
                                when offer.isCapitec=1 then 8
                                when offer.isComcorp=1 then 9
                                else os.OriginationSourceKey
                           end,
    OriginationSource = case
                                when offer.isCapitec = 1 then 'Capitec'
                                when offer.isComcorp=1 then 'Comcorp'
                                else os.[Description]
                        end,
    isInArrears = case
                    when Arrear.ArrearBalance > 0 then 1
                    else 0
                end,
    isInAdvance = case
                    when Arrear.ArrearBalance < 0 then 1
                    else 0
                end,
    isnull(fa.FinancialAdjustmentStatusKey,0) as isNonPerforming,
    sa.Description as AccountStatus,
    ac.OpenDate,
    ac.CloseDate,
    p.Description as Product,
    spv.Description as CurrentSPV,
    Bonds.TotalBondAmount,
    Bonds.LoanAgreement,
    CurrentLTV = case
            when (fin.Balance / v.ValuationAmount) > 0 then (fin.Balance / v.ValuationAmount)
            else 0.0
          end,
    CurrentPTI = case
            when (fin.Instalment / oic.OriginatingOfferIncomeContributorTotalConfirmedIncome) > 0
                then (fin.Instalment / oic.OriginatingOfferIncomeContributorTotalConfirmedIncome)
            else 0.0
          end,
    fsba.DebitOrderDay,
    fspt.Description as PaymentType,
    oic.OriginatingOfferIncomeContributorTotalConfirmedIncome,
    hic.HouseholdTotalConfirmedIncome,
    fs.NextResetDate,
    fin.EffectiveRate,
    fin.FixedRate,
    lb.RemainingInstalments as RemainingTerm,
    fin.Instalment,
    fin.Balance as CurrentBalance,
    Arrear.ArrearBalance
from [2am].[dbo].[Account] ac
join [2am].[dbo].[OriginationSourceProduct] osp on osp.OriginationSourceProductKey=ac.OriginationSourceProductKey
join [2am].[dbo].[OriginationSource] os on os.OriginationSourceKey=osp.OriginationSourceKey
join [2am].[dbo].[AccountStatus] sa on sa.AccountStatusKey=ac.[AccountStatusKey]
join [2am].[dbo].[Product] p on p.ProductKey=ac.RRR_ProductKey
join [2am].[spv].SPV spv on spv.SPVKey=ac.spvkey
join [2am].[dbo].[FinancialService] fs on fs.AccountKey=ac.AccountKey
join [2am].[fin].[MortgageLoan] ml on ml.FinancialServiceKey=fs.FinancialServiceKey
join [2am].[dbo].[FinancialServiceType] fst on fst.FinancialServiceTypeKey=fs.FinancialServiceTypeKey and fst.BalanceTypeKey=1
join [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
join [2am].[dbo].[FinancialServiceBankAccount] fsba on fsba.FinancialServiceKey=fs.FinancialServiceKey and fsba.GeneralStatusKey=1
join [2am].[dbo].[FinancialServicePaymentType] fspt on fspt.FinancialServicePaymentTypeKey=fsba.FinancialServicePaymentTypeKey
join [2am].[fin].LoanBalance lb on lb.FinancialServiceKey=fs.FinancialServiceKey
left join [2am].[fin].[FinancialAdjustment] fa on fa.FinancialServiceKey=fs.FinancialServiceKey and
                                                  fa.FinancialAdjustmentStatusKey=1 and fa.FinancialAdjustmentSourceKey=2 -- non-performing
left join [2am].[fin].[Fee] fee on fs.FinancialServiceKey=fee.FinancialServiceKey
left join [2am].[dbo].[Valuation] v on v.PropertyKey=ml.PropertyKey and v.IsActive=1
join 
    (select
        fs.accountkey,
        (sum(fs.Payment) + sum(isnull(fee.Amount,0.0))) as Instalment,
        sum(b.Amount) as Balance,
        sum(case when(fs.FinancialServiceTypeKey = 1) then lb.InterestRate end) as EffectiveRate,
        sum(case when(fs.FinancialServiceTypeKey = 2) then lb.InterestRate else 0 end) as FixedRate
    from [2am].[dbo].[FinancialService] fs
    join [2am].[dbo].[FinancialServiceType] fst on fst.FinancialServiceTypeKey=fs.FinancialServiceTypeKey and fst.BalanceTypeKey=1
    join [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
    left join[2am].[fin].[Fee] fee on fs.FinancialServiceKey=fee.FinancialServiceKey
    left join [2am].[fin].LoanBalance lb on b.FinancialServiceKey = lb.FinancialServiceKey
    where fs.AccountKey={0}
    group by fs.accountkey
    ) fin on fin.Accountkey=ac.accountkey
join
    (select
        fs.accountkey,
        sum(b.Amount) as ArrearBalance
    from [2am].[dbo].[FinancialService] fs
    join [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=4
    where
        fs.AccountKey={0}
        and fs.FinancialServiceTypeKey=8
    group by fs.AccountKey
    ) Arrear on Arrear.Accountkey=ac.accountkey
join
    (select
        o.accountkey,
        isCapitec = case
                        when isnull(oa.offerattributetypekey,-1)=30 then 1
                        else 0
                    end,
        isComcorp = case
                        when isnull(oa.offerattributetypekey,-1)=31 then 1
                        else 0
                    end
    from [2am].dbo.offer o
    left join [2am].dbo.offerattribute oa on oa.OfferKey=o.offerkey
    where o.accountkey={0}
        and o.offertypekey in (6,7,8) --originating offer
    ) offer on offer.accountkey=ac.accountkey
left join
    (select
        fs.accountkey,
        sum(b.BondRegistrationAmount) as TotalBondAmount,
        sum(b.BondLoanAgreementAmount) as LoanAgreement
    from [2am].[dbo].[FinancialService] fs
    join [2am].[fin].[MortgageLoan] pml on pml.FinancialServiceKey=fs.FinancialServiceKey
    join [2am].[dbo].[BondMortgageLoan] bml on bml.FinancialServiceKey=fs.FinancialServiceKey
    join [2am].[dbo].[Bond] b on b.BondKey=bml.BondKey
    where fs.AccountKey = {0}
    group by fs.AccountKey
    ) Bonds on Bonds.AccountKey = ac.accountkey
left join
    (select
        o.accountkey, sum(ConfirmedIncome) as OriginatingOfferIncomeContributorTotalConfirmedIncome
    from [2am].dbo.offer o
    left join [2am].dbo.offerrole ro on ro.offerkey=o.offerkey
    left join [2am].dbo.employment e on e.LegalEntityKey=ro.LegalEntityKey
    left join [2am].dbo.offerroleattribute roa on roa.offerrolekey=ro.offerrolekey and roa.OfferRoleAttributeTypeKey=1
    left join [2am].dbo.offerroletype ort on ort.offerroletypekey=ro.offerroletypekey
    left join [2am].dbo.offerroletypegroup ortg on ortg.offerroletypegroupkey=ort.offerroletypegroupkey
    where o.accountkey={0}
            and o.offertypekey in (6,7,8) --originating offer
            and ro.generalstatuskey=1
            and ortg.offerroletypegroupkey=3
            and employmentenddate is null
    group by o.accountkey
    ) oic on oic.accountkey=ac.accountkey
left join
    (
    select
        r.accountkey, sum(ConfirmedIncome) as HouseholdTotalConfirmedIncome
    from [2am].dbo.role r
    left join [2am].dbo.employment e on e.LegalEntityKey=r.LegalEntityKey
    where r.accountkey={0}
            and r.generalstatuskey=1
            and employmentenddate is null
    group by r.accountkey
    ) hic on hic.accountkey=ac.accountkey
where ac.[AccountKey]={0}", businessContext.BusinessKey.Key);
        }
    }
}