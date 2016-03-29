using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.MortgageLoan
{
    public class MortgageLoanChildDataProvider : HaloTileBaseChildDataProvider,
                                                 IHaloTileChildDataProvider<MortgageLoanChildTileConfiguration>
    {
        public MortgageLoanChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.Offer;
            return string.Format(@"select DISTINCT a.accountkey AS BusinessKey, {1} as BusinessKeyType from [2am].[dbo].[role] r
                                    inner join [2am].[dbo].[account] a on a.accountkey = r.accountkey
                                    where a.accountstatuskey = 1
                                    and a.rrr_productkey in (1,2,5,9,11)
                                    and a.ParentAccountKey is null
                                    and legalentitykey = {0}", businessContext.BusinessKey.Key, genericKeyType);
        }
    }
}