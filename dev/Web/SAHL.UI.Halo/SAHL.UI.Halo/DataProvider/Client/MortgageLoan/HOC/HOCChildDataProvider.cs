using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan.HOC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.MortgageLoan.HOC
{
    public class HOCChildDataProvider : HaloTileBaseChildDataProvider,
                                                 IHaloTileChildDataProvider<HOCChildTileConfiguration>
    {
        public HOCChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Account;
            return string.Format(@"select DISTINCT a.accountkey AS BusinessKey, {1} as BusinessKeyType from [2am].[dbo].[role] r
                                    inner join [2am].[dbo].[account] a on a.accountkey = r.accountkey
                                    where a.rrr_productkey = 3
                                    AND ParentAccountKey = {0}", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}