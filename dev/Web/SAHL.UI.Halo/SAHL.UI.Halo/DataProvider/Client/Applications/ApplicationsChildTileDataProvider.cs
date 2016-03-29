using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Client.Applications;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.Applications
{
    public class ApplicationsChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<ApplicationsChildTileConfiguration>
    {
        public ApplicationsChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}