using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Models
{
    public interface IActionTileModel : ITileModel, ITileContentProvider
    {
        string Title { get; }

        string Icon { get; }

        string BottomLeftOverlayIcon { get; }

        string BottomRightOverlayIcon { get; }
    }
}