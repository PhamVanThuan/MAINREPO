using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public class StaticRibbonMenuItemElement : AbstractRibbonMenuItemElement
    {
        public StaticRibbonMenuItemElement(string idPostfix, string url)
            : base(idPostfix, url, UrlAction.LinkNavigation)
        {
        }
    }
}