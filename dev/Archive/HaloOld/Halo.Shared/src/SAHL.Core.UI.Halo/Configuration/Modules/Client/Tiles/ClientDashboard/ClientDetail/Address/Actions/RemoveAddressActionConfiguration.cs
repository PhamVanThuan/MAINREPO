using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Actions.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.Address.Actions
{
    public class RemoveAddressActionConfiguration : ActionTileConfiguration<RemoveAddressActionTileModel>,
                                                    IParentedActionTileConfiguration<LegalEntityAddressDrillDownTileConfiguration>
    {
        public RemoveAddressActionConfiguration()
            : base("RemoveAddressActionAccess", 0, UrlNames.EditorController, UrlNames.StartEditorAction, UrlAction.TileLaunchEditor)
        {

        }
    }
}
