using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Application;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Application
{
    public class NewPurchaseApplicationRootTileContentDataProvider : HaloTileBaseContentDataProvider<NewPurchaseApplicationRootModel>
    {
        public NewPurchaseApplicationRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"
select o.OfferKey as ApplicationNumber, 
       isnull(o.AccountKey,o.ReservedAccountKey) as AccountKey,
       p.Description as Product,
       at.Description as ApplicantType, 
       et.Description as EmploymentType, 
       oivl.HouseholdIncome as HouseholdIncome, 
       oml.PurchasePrice,
       round(oivl.CashDeposit,2) as CashDeposit,
       round(oivl.FeesTotal,2) as TotalFees, 
       round(oivl.LoanAgreementAmount,2) as LoanAgreementAmount,
       case
            when v.ValuationKey is null then oml.PurchasePrice
            else v.ValuationAmount
       end as PropertyValuation,
       mr.Value as MarketRate,
       isnull(m.Value,0.0) + isnull(PricingAdjustments.PricingAdjustment,0.0)  as EffectiveLinkRate,
       isnull(m.Value,0.0) + isnull(PricingAdjustments.PricingAdjustment,0.0) + mr.Value as EffectiveInterestRate,
       oivl.Term as Term, 
       oivl.MonthlyInstalment,
       round(isnull(oivl.LTV,0.0),4) as LTV, 
       round(isnull(oivl.PTI,0.0),4) as PTI, 
       spv.ReportDescription as SPV,
       ott.Description as OccupancyType
from   [2am].[dbo].Offer o 
       left join [2am].[dbo].OfferInformation oi on oi.OfferInformationKey = (select top 1 OfferInformationKey from  [2am].[dbo].OfferInformation where OfferKey=o.OfferKey order by ChangeDate desc)
       left join [2am].[dbo].OfferInformationType oit on oi.OfferInformationTypeKey=oit.OfferInformationTypeKey
       left join [2am].[dbo].OfferInformationVariableLoan oivl on oi.offerinformationkey = oivl.offerinformationkey 
       left join [2am].[dbo].OfferMortgageLoan oml on o.offerkey = oml.offerkey 
       left join [2am].[dbo].ApplicantType at on oml.applicanttypekey = at.applicanttypekey
       left join [2am].[dbo].RateConfiguration rc on oivl.rateconfigurationkey = rc.rateconfigurationkey 
       left join [2am].[dbo].Margin m on rc.marginkey = m.marginkey 
       left join [2am].[dbo].MarketRate mr on rc.marketratekey = mr.marketratekey 
       left join [2am].[dbo].Product p on oi.productkey = p.productkey 
       left join [2am].[dbo].OfferType ot on o.offertypekey = ot.offertypekey 
       left join [2am].[dbo].MortgageLoanPurpose mlp on oml.mortgageloanpurposekey = mlp.mortgageloanpurposekey 
       left join [2am].[dbo].EmploymentType et on oivl.employmenttypekey = et.employmenttypekey 
       left join [2am].[dbo].Offerexpense initfee on o.offerkey = initfee.offerkey  AND initfee.expensetypekey = 1 
       left join [2am].[dbo].Offerexpense regfee on o.offerkey = regfee.offerkey AND regfee.expensetypekey = 4 
       left join [2am].[spv].SPV on spv.SPVKey = oivl.SPVKey
       left join [2am].[dbo].Property pp on pp.PropertyKey=oml.PropertyKey
       left join [2am].[dbo].OccupancyType ott on ott.OccupancyTypeKey=pp.OccupancyTypeKey
       left join [2am].[dbo].Valuation v on v.PropertyKey=oml.PropertyKey and v.IsActive = 1 -- active Valuation
       left join [2am].[dbo].Offerattribute oa  on o.offerkey = oa.offerkey  AND oa.offerattributetypekey = 3  -- Capitalize fees
       left join [2am].[dbo].Offerattribute qpoa  on o.offerkey = qpoa.offerkey  AND qpoa.offerattributetypekey = 25 -- QuickPay Loan
       left join [2am].[dbo].OfferInformationInterestOnly oiio on oi.offerinformationkey = oiio.offerinformationkey 
       left join [2am].[dbo].OfferInformationVariFixLoan oivf on oi.offerinformationkey = oivf.offerinformationkey 
       left join ( select SUM(oifa.discount) as [PricingAdjustment], oi.OfferInformationKey 
                   from [2am].[dbo].OfferInformation oi 
                   join [2am].[dbo].offerinformationfinancialadjustment oifa on oi.OfferInformationKey=oifa.OfferInformationKey
                   where oi.OfferKey={0} group by oi.OfferInformationKey
                 ) as PricingAdjustments on PricingAdjustments.OfferInformationKey = oi.OfferInformationKey
where  o.offerkey = {0}", businessContext.BusinessKey.Key);
        }
    }
}
