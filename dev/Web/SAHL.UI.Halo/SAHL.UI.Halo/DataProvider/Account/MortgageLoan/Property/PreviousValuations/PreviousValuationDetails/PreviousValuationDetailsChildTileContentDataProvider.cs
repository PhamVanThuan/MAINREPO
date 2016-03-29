using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.PreviousValuations.PreviousValuationDetails
{
    public class PreviousValuationDetailsChildTileContentDataProvider : HaloTileBaseContentDataProvider<PreviousValuationModel>
    {
        public PreviousValuationDetailsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT
                    [2am].[dbo].LegalEntityLegalName(vv.LegalEntityKey, 0) AS Valuer,
                    ValuationAge = dbo.fCalculateAge(v.ValuationDate, getdate()),
                    v.ValuationDate,
                    v.ValuationAmount,
					v.ValuationMunicipal,
                    v.ValuationHOCValue,
                    hr.Description as HOCRoofType,
					dp.Description as ValuationProvider
                FROM
                    [2am].[dbo].[Valuation] v
                    left join [2am].[dbo].[Valuator] vv on vv.ValuatorKey=v.ValuatorKey
                    join [2am].[dbo].[HOCRoof] hr on hr.HOCRoofKey=v.HOCRoofKey
					join [2am].[dbo].ValuationDataProviderDataService vdpds on v.ValuationDataProviderDataServiceKey = vdpds.ValuationDataProviderDataServiceKey
					join [2am].[dbo].DataProviderDataService dpds on vdpds.DataProviderDataServiceKey = dpds.DataProviderDataServiceKey
					join [2am].[dbo].DataProvider dp on dpds.DataProviderKey = dp.DataProviderKey
                WHERE
                    v.ValuationKey = {0} and
                    v.isActive = 0", businessContext.BusinessKey.Key);
        }
    }
}