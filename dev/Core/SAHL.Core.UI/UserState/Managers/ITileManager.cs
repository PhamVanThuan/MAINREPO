using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Areas;
using SAHL.Core.UI.UserState.Models;

namespace SAHL.Core.UI.UserState.Managers
{
    public interface ITileManager
    {
        TileElementArea LoadUserTilesForBusinessContext(IUserPrincipal user, TileBusinessContext tileBusinessContext);

        TileElementArea DrillDownAndLoadUserTilesForBusinessContext(IUserPrincipal user, TileBusinessContext tileBusinessContext);

        dynamic GetTileContent(TileBusinessContext tileBusinessContext);

        string GetContextDescriptionForMajorTileContext(TileBusinessContext tileBusinessContext);
    }
}