using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.HOC;
using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.MortgageLoan.HOC
{
    public class HOCChildTileContentDataProvider : HaloTileBaseContentDataProvider<HOCChildModel>
    {
        public HOCChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select hi.Description as HOCInsurer,
                    ha.AccountStatusKey hocAccountStatus,
                    ha.AccountKey as PolicyNumber,
                    hoc.HOCPolicyNumber,
                    case when hi.HOCInsurerKey = 2 then 1 else 0 end as IsSAHLHOC,
                    [2am].dbo.fGetFormattedAddressDelimited(p.AddressKey,0) as PropertyAddress,
                    hoc.HOCTotalSumInsured as SumInsured,
                    v.ValuationDate,
                    v.ValuationHOCValue as ActiveValuationHOCValue,
                    hoc.CommencementDate, 
                    hoc.AnniversaryDate,
                    hoc.Ceded,
                    (fs.Payment + isnull(fee.Amount,0.0)) as CurrentTotalMonthlyHOCPaymentExposureToSAHL
                    from [2am].[dbo].[FinancialService] fs 
                    join [2am].[dbo].[HOC] hoc on fs.FinancialServiceKey=hoc.FinancialServiceKey
                    join [2am].[dbo].[HOCInsurer] hi on hi.HOCInsurerKey=hoc.HOCInsurerKey
                    join [2am].[dbo].[Account] ha on ha.AccountKey=fs.AccountKey
                    join [2am].[dbo].[Account] mla on mla.AccountKey=ha.ParentAccountKey
                    join [2am].[dbo].[FinancialService] mlfs on mlfs.AccountKey=mla.AccountKey
                    join [2am].[fin].[MortgageLoan] ml on ml.FinancialServiceKey=mlfs.FinancialServiceKey
                    join [2am].[dbo].[Property] p on p.PropertyKey=ml.PropertyKey
                    join [2am].[dbo].[FinancialServiceType] fst on fst.FinancialServiceTypeKey=fs.FinancialServiceTypeKey and fst.BalanceTypeKey=1
                    join [2am].[fin].[Balance] b on b.FinancialServiceKey=fs.FinancialServiceKey and b.BalanceTypeKey=1
                    left join [2am].[dbo].[Valuation] v on v.PropertyKey=ml.PropertyKey and v.IsActive=1
                    left join [2am].[fin].[Fee] fee on fs.FinancialServiceKey=fee.FinancialServiceKey
                    where fs.AccountKey={0}", businessContext.BusinessKey.Key);
        }
    }
}