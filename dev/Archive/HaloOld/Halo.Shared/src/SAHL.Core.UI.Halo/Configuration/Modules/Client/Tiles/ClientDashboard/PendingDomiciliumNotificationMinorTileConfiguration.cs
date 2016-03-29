using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Halo.Tiles.Notification.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard
{
    public class PendingDomiciliumNotificationMinorTileConfiguration : MinorTileConfiguration<PendingDomiciliumNotificationMinorTileModel>, IParentedTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public PendingDomiciliumNotificationMinorTileConfiguration()
            : base("PendingDomiciliumNotificationMinorTileAccess", 0)
        {

        }
    }
}
