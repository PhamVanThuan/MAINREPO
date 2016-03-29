using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Providers.Tiles;
using SAHL.Core.UI.UserState.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public interface ITileConfigurationManager
    {
        IRootTileConfiguration GetRootTileConfigurationForContext(IPrincipal user, string context);

        IEnumerable<IChildTileConfiguration> GetChildConfigurationsForMajorTile(IMajorTileConfiguration majorTileConfiguration);

        IEnumerable<IActionTileConfiguration> GetActionTileConfigurationsForMajorTile(IMajorTileConfiguration majorTileConfiguration);

        ITileDataProvider GetTileDataProviderForTileModel(Type tileModelType);

        ITileContentProvider GetTileContentProviderForTileModel(Type tileModelType);

        IMajorTileConfiguration GetDrillDownConfigurationForClickedTile(Type clickedTileConfigurationType);

        IMajorTileConfiguration GetTileConfigurationFromType(Type contextConfigurationType);

        Type GetAlternateTileModelIfConfiguredForUser(Type defaultTileModel, IUserPrincipal userPrincipal);
    }
}