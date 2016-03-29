using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsRootDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<ApplicationsRootTileConfiguration>
    {
        public ApplicationsRootDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
            
        }
       
    }
}