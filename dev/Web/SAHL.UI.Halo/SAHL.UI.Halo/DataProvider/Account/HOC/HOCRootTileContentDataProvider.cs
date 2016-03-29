using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.HOC
{
    public class HOCRootTileContentDataProvider : HaloTileBaseContentDataProvider<HOCRootModel>
    {
        public HOCRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
    AccountNumber = {0},
    OriginationSourceKey = os.OriginationSourceKey,
    OriginationSource = os.[Description],
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
    hoc.CommencementDate,
    hoc.AnniversaryDate,
    hoc.HOCPolicyNumber,
    hoc.HOCMonthlyPremium,
    hoc.HOCTotalSumInsured,
    hr.Description as RoofDescription,
    hi.Description as HOCInsurer
from [2am].[dbo].[Account] ac
join [2am].[dbo].[OriginationSourceProduct] osp on osp.OriginationSourceProductKey=ac.OriginationSourceProductKey
join [2am].[dbo].[OriginationSource] os on os.OriginationSourceKey=osp.OriginationSourceKey
join [2am].[dbo].[AccountStatus] sa on sa.AccountStatusKey=ac.[AccountStatusKey]
join [2am].[dbo].[FinancialService] fs on fs.AccountKey=ac.AccountKey
join [2am].[dbo].[FinancialServiceType] fst on fst.FinancialServiceTypeKey=fs.FinancialServiceTypeKey and fst.BalanceTypeKey=1
join [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
join [2am].[dbo].[HOC] hoc on hoc.FinancialServiceKey=fs.FinancialServiceKey
join [2am].[dbo].[HOCRoof] hr on hr.HOCRoofKey=hoc.HOCRoofKey
join [2am].[dbo].[HOCInsurer] hi on hi.HOCInsurerKey=hoc.HOCInsurerKey
join
    (select
        fs.accountkey,
        b.Amount as ArrearBalance
    from [2am].[dbo].[FinancialService] fs
    join [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=4
    where
        fs.AccountKey={0}
        and fs.FinancialServiceTypeKey=8
    ) Arrear on Arrear.Accountkey=ac.accountkey
left join [2am].[fin].[FinancialAdjustment] fa on fa.FinancialServiceKey=fs.FinancialServiceKey
                                               and fa.FinancialAdjustmentStatusKey=1
                                               and fa.FinancialAdjustmentSourceKey=2 -- non-performing
where ac.AccountKey={0}", businessContext.BusinessKey.Key);
        }
    }
}