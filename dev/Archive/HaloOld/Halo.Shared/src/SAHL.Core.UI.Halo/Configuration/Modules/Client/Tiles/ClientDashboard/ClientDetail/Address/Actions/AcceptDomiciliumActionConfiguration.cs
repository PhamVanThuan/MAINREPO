﻿using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Actions.Address;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.Address.Actions
{
    public class AcceptDomiciliumActionConfiguration : ActionTileConfiguration<AcceptDomiciliumActionTileModel>,
                                                    IParentedActionTileConfiguration<LegalEntityAddressDrillDownTileConfiguration>
    {
        public AcceptDomiciliumActionConfiguration()
            : base("AcceptDomiciliumActionAccess", 0, UrlNames.EditorController, UrlNames.StartEditorAction, UrlAction.TileLaunchEditor)
        {
        }
    }
}