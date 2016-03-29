using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations;
using SAHL.UI.Halo.Configuration.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.PreviousValuations
{
    public class PreviousValuationsChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<PreviousValuationsChildTileConfiguration>
    {
        public PreviousValuationsChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Property;
            return string.Format(@"SELECT top 1 PropertyKey as BusinessKey, 
                                        {1} as BusinessKeyType 
                            FROM 
                                 [2am].[dbo].[Valuation] v
                            WHERE 
                                 v.PropertyKey = {0} and v.IsActive = 0", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}