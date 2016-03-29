using Newtonsoft.Json;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public abstract class AbstractMenuItemElement : VisualElement, IVisualElement, ISelectable, IUrlElement
    {
        public AbstractMenuItemElement(string idPostfix, string url, UrlAction urlAction)
            : base(string.Format("{0}_{1}", ElementNames.MenuItemIdPrefix, idPostfix != null ? idPostfix.ToLower() : ""))
        {
            this.Url = url;
            this.UrlAction = urlAction;
        }

        [JsonIgnore]
        public bool Selected
        {
            get
            {
                return this.SelectionContext.CurrentSelection != null && this.SelectionContext.IsItemSelected(this);
            }
        }

        public ISelectionContext SelectionContext { get; set; }

        public string Url { get; protected set; }

        public UrlAction UrlAction { get; protected set; }
    }
}