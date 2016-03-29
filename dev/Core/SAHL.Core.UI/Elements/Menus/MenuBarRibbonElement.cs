using Newtonsoft.Json;
using SAHL.Core.BusinessModel;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.UI.Elements.Menus
{
    public class MenuBarRibbonElement : VisualElement, ISelectionContext
    {
        [JsonIgnore]
        private List<AbstractRibbonMenuItemElement> ribbonMenuItemElements;

        [JsonProperty]
        private AbstractRibbonMenuItemElement currentSelection;

        public MenuBarRibbonElement()
            : base("menubar-ribbon")
        {
            ribbonMenuItemElements = new List<AbstractRibbonMenuItemElement>();
        }

        public IEnumerable<AbstractRibbonMenuItemElement> RibbonMenuItemElements
        {
            get { return ribbonMenuItemElements; }
        }

        public ISelectable CurrentSelection
        {
            get { return this.currentSelection; }
        }

        public void AddRibbonMenuElement(AbstractRibbonMenuItemElement element)
        {
            ribbonMenuItemElements.Add(element);
            if (this.ribbonMenuItemElements.Count == 1)
            {
                this.currentSelection = element;
            }
            element.SelectionContext = this;
        }

        public void AddMenuElementRange(IEnumerable<AbstractRibbonMenuItemElement> elements)
        {
            this.ribbonMenuItemElements.AddRange(elements);
            if (this.currentSelection == null && this.ribbonMenuItemElements.Count > 0)
            {
                this.currentSelection = this.ribbonMenuItemElements[0];
            }

            foreach (var element in elements)
            {
                element.SelectionContext = this;
            }
        }

        public void RemoveRibbonMenuElement(BusinessContext businessContext)
        {
            var ribbonMenuItem = ribbonMenuItemElements.Where(x => x.BusinessContext == businessContext).SingleOrDefault();
            ribbonMenuItemElements.Remove(ribbonMenuItem);
        }

        public bool IsItemSelected(ISelectable selectableItem)
        {
            return selectableItem == this.currentSelection;
        }

        public void Select(ISelectable itemToSelect)
        {
            AbstractRibbonMenuItemElement ribbonItemToSelect = itemToSelect as AbstractRibbonMenuItemElement;
            if (ribbonItemToSelect != null && this.ribbonMenuItemElements.Contains(ribbonItemToSelect))
            {
                this.currentSelection = ribbonItemToSelect;
            }
        }
    }
}