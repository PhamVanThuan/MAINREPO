using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.ActiveValuation;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.DeedsOfficeDetails;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.ActiveValuation
{
    public class ActiveValuationChildDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<ActiveValuationChildTileConfiguration>
    {
        public ActiveValuationChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Valuation;
            return string.Format(@"SELECT 
                                        v.ValuationKey as BusinessKey, 
                                        {1} as BusinessKeyType
                                    FROM 
                                        [2am].[dbo].[Valuation] v
                                     WHERE 
                                        v.PropertyKey = {0}
                                        and v.isActive = 1", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}
