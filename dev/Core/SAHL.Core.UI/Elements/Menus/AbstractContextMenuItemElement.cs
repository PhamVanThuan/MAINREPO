using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public class AbstractContextMenuItemElement : VisualElement, IVisualElement, ISelectable, IUrlElement
    {
        public AbstractContextMenuItemElement(BusinessContext businessContext, string url, UrlAction urlAction)
            : base(ElementNames.ContextMenuItemIdPrefix, businessContext)
        {
            this.Url = url;
            this.UrlAction = urlAction;
        }

        public AbstractContextMenuItemElement(string idPostfix, string url, UrlAction urlAction)
            : base(string.Format("{0}_{1}", ElementNames.ContextMenuItemIdPrefix, idPostfix))
        {
            this.Url = url;
            this.UrlAction = urlAction;
        }

        public bool Selected { get; protected set; }

        public ISelectionContext SelectionContext { get; set; }

        public string Url { get; protected set; }

        public UrlAction UrlAction { get; protected set; }
    }
}