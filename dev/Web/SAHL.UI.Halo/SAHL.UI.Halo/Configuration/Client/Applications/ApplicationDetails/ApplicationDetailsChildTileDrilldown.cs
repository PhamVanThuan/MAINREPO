using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Application;

namespace SAHL.UI.Halo.Configuration.Client.Applications.ApplicationDetails
{
    public class ApplicationDetailsChildTileDrilldown : HaloTileActionDrilldownBase<ApplicationDetailsChildTileConfiguration, ApplicationRootTileConfiguration>,
                                                        IHaloTileActionDrilldown<ApplicationDetailsChildTileConfiguration>
    {
        public ApplicationDetailsChildTileDrilldown()
            : base("Application")
        {
        }
    }
}
