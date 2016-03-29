using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Detail.EmploymentSummary;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail.EmploymentSummary
{
    public class EmploymentSummaryChildDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<EmploymentSummaryChildTileConfiguration>
    {
        public EmploymentSummaryChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select distinct 
                                (e.LegalEntityKey) as BusinessKey, 0 as BusinessKeyType
                            from
                                [2am].[dbo].[Employment] e
                            where
                                e.LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}