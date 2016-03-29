using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client
{
    public class ClientRootTileDataProvider : HaloTileBaseChildDataProvider,
                                              IHaloTileChildDataProvider<ClientRootTileConfiguration>
    {
        public ClientRootTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}