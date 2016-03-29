using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public class DynamicRibbonMenuItemElement : AbstractRibbonMenuItemElement
    {
        public DynamicRibbonMenuItemElement(BusinessContext businessContext, string url)
            : base(businessContext, url, UrlAction.LinkNavigation)
        {
        }
    }
}