using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Detail;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail
{
    public class ClientDetailRootTileDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<ClientDetailRootTileConfiguration>
    {
        public ClientDetailRootTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}