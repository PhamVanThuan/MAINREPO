using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.ITC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.ITC
{
    public class ITCChildDataProvider : HaloTileBaseChildDataProvider,
                                       IHaloTileChildDataProvider<ITCChildTileConfiguration>
    {
        public ITCChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select distinct (itc.LegalEntityKey) as BusinessKey, 0 as BusinessKeyType
                            from
                                [2am].[dbo].[ITC] itc
                            where
                                itc.LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}