using SAHL.Core.BusinessModel;
using SAHL.Core.Data;

using SAHL.UI.Halo.Models.Common.Correspondence;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Correspondence
{
    public class CorrespondenceChildTileContentDataProvider : HaloTileBaseContentDataProvider<CorrespondenceChildModel>
    {
        public CorrespondenceChildTileContentDataProvider(IDbFactory dbFactory)
           : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return "";
        }
    }
}