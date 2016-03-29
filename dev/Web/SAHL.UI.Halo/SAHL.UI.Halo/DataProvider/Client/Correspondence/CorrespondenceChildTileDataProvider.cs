using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Correspondence;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Correspondence
{
    public class CorrespondenceChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<CorrespondenceChildTileConfiguration>
    {
        public CorrespondenceChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}