using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.PreviousValuations.PropertyDetails
{
    public class PreviousValuationsRootTileContentDataProvider : HaloTileBaseContentDataProvider<PropertyRootModel>
    {
        public PreviousValuationsRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT
                                   dp.[Description] AS DataProvider, -- this can be iconf for SAHL, LightStone or Adcheck
                                   [2am].[dbo].fGetFormattedAddressDelimited(p.AddressKey, 1) as PropertyAddress,
                                   ot.Description AS Occupancy,
                                   pt.Description AS PropertyType,
                                   tt.Description AS TitleType,
                                   p.PropertyDescription1 AS LegalDescription1,
                                   p.PropertyDescription2 AS LegalDescription2,
                                   p.PropertyDescription3 AS LegalDescription3,
                                   ac.[Description] AS AreaClassification
                                FROM
                                   [2am].[dbo].[Property] p
                                   join [2am].[dbo].[OccupancyType] ot on ot.OccupancyTypeKey=p.OccupancyTypeKey
                                   join [2am].[dbo].[TitleType] tt on tt.TitleTypeKey=p.TitleTypeKey
                                   join [2am].[dbo].[PropertyType] pt on pt.PropertyTypeKey=p.PropertyTypeKey
                                   join [2am].[dbo].[DataProvider] dp on dp.DataProviderKey=p.DataProviderKey
                                   join [2am].[dbo].[AreaClassification] ac on ac.AreaClassificationKey = p.AreaClassificationKey
                                WHERE
                                    p.PropertyKey={0}", businessContext.BusinessKey.Key);
        }
    }
}