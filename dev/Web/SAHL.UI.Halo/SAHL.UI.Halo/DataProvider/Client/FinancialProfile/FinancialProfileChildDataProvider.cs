using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.FinancialProfile;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.FinancialProfile
{
    public class FinancialProfileChildDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<FinancialProfileChildTileConfiguration>
    {
        public FinancialProfileChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select distinct (r.LegalEntityKey) as BusinessKey, 0 as BusinessKeyType
                            from
                                [2am].[dbo].[Role] r
                            where
                                r.LegalEntityKey = {0}
                            and r.generalstatuskey = 1", businessContext.BusinessKey.Key);
        }
    }
}