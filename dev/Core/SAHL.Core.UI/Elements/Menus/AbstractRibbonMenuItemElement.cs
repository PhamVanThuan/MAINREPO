using Newtonsoft.Json;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public abstract class AbstractRibbonMenuItemElement : VisualElement, IVisualElement, ISelectable, IUrlElement
    {
        public AbstractRibbonMenuItemElement(BusinessContext businessContext, string url, UrlAction urlAction)
            : base(ElementNames.RibbonMenuItemIdPrefix, businessContext)
        {
            this.Url = url;
            this.UrlAction = urlAction;
        }

        public AbstractRibbonMenuItemElement(string idPostfix, string url, UrlAction urlAction)
            : base(string.Format("{0}_{1}", ElementNames.RibbonMenuItemIdPrefix, idPostfix))
        {
            this.Url = url;
            this.UrlAction = urlAction;
        }

        [JsonIgnore]
        public bool Selected
        {
            get
            {
                return this.SelectionContext.IsItemSelected(this);
            }
        }

        public ISelectionContext SelectionContext { get; set; }

        public string Url { get; protected set; }

        public UrlAction UrlAction { get; protected set; }
    }
}