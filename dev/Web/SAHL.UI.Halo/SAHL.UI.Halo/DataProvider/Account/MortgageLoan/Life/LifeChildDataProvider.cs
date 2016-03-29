using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Life
{
    public class LifeChildDataProvider : HaloTileBaseChildDataProvider,
                                         IHaloTileChildDataProvider<LifeChildTileConfiguration>
    {
        public LifeChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Account;
            return string.Format(@"select
                la.AccountKey as BusinessKey,
                {1} as GenericKeyType
                from [2am].[dbo].[Account] la 
                join [2am].[dbo].[FinancialService] fs on fs.AccountKey = la.AccountKey and fs.FinancialServiceTypeKey = 5
                join [2am].[dbo].[LifePolicy] lp on lp.FinancialServiceKey = fs.FinancialServiceKey
                where la.ParentAccountKey = {0}", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}
