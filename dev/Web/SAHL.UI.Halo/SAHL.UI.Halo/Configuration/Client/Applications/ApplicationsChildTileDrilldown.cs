
using SAHL.UI.Halo.Configuration.Client.Applications.Application;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Client.Applications
{
    public class ApplicationsChildTileDrilldown : HaloTileActionDrilldownBase<ApplicationsChildTileConfiguration, ApplicationsRootTileConfiguration>,
                                                  IHaloTileActionDrilldown<ApplicationsChildTileConfiguration>
    {
        public ApplicationsChildTileDrilldown()
            : base("ApplicationDetails")
        {
        }
    }
}
