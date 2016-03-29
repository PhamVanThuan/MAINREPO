using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public class StaticMenuItemElement : AbstractMenuItemElement
    {
        public StaticMenuItemElement(string idPostfix, string url)
            : base(idPostfix, url, UrlAction.LinkNavigation)
        {
        }
    }
}