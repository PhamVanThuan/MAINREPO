using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Models
{
    public abstract class AbstractActionTileModel<T> : IActionTileModel, ITileContentProvider<T> where T : IActionTileModel
    {
        public AbstractActionTileModel(string title, string icon)
            : this(title, icon, string.Empty, string.Empty)
        {
        }

        public AbstractActionTileModel(string title, string icon, string bottomLeftOverlayIcon, string bottomRightOverlayIcon)
        {
            this.Title = title;
            this.Icon = icon;
            this.BottomLeftOverlayIcon = bottomLeftOverlayIcon;
            this.BottomRightOverlayIcon = bottomRightOverlayIcon;
        }

        public string Title { get; protected set; }

        public string Icon { get; protected set; }

        public string BottomLeftOverlayIcon { get; protected set; }

        public string BottomRightOverlayIcon { get; protected set; }

        public dynamic GetContent(BusinessModel.BusinessKey businessKey)
        {
            return this;
        }
    }
}