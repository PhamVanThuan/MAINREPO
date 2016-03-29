using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property
{
    public class PropertyChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<PropertyChildTileConfiguration>
    {
        public PropertyChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }
        public override string GetSqlStatement(BusinessContext businessContext)
        {

            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Property;
            return string.Format(@"SELECT DISTINCT 
                                 p.PropertyKey as BusinessKey, 
                                 {1} as BusinessKeyType
                                 FROM
                                      [2am].[dbo].[FinancialService] fs
                                      JOIN [2am].[fin].[MortgageLoan] ml ON ml.FinancialServiceKey=fs.FinancialServiceKey
                                      JOIN [2am].[dbo].[Property] p ON p.PropertyKey=ml.PropertyKey
                                  WHERE
                                   fs.AccountKey = {0}", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}