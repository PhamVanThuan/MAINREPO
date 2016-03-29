using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.PreviousValuations
{
    public class PreviousValuationsChildTileContentDataProvider : HaloTileBaseContentDataProvider<PreviousValuationsModel>
    {
        public PreviousValuationsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT COUNT(*) as NumPreviousValuations 
                            FROM 
                                 [2am].[dbo].[Valuation] v
                            WHERE 
                                 v.PropertyKey = {0} and v.IsActive = 0", businessContext.BusinessKey.Key);
        }
    }
}