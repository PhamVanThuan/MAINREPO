using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAHL.Core.UI.Elements.Menus
{
    public class MenuBarMenuElement : VisualElement, ISelectionContext
    {
        [JsonIgnore]
        private List<AbstractMenuItemElement> menuItemElements;

        [JsonProperty]
        private AbstractMenuItemElement currentSelection;

        public MenuBarMenuElement()
            : base("menubar-menu")
        {
            this.menuItemElements = new List<AbstractMenuItemElement>();
        }

        public IEnumerable<AbstractMenuItemElement> MenuItemElements
        {
            get { return this.menuItemElements; }
        }

        public ISelectable CurrentSelection
        {
            get { return this.currentSelection; }
        }

        public void AddMenuElement(AbstractMenuItemElement element)
        {
            this.menuItemElements.Add(element);
            if (this.menuItemElements.Count == 1)
            {
                this.currentSelection = element;
            }
            element.SelectionContext = this;
        }

        public void AddMenuElementRange(IEnumerable<AbstractMenuItemElement> elements)
        {
            this.menuItemElements.AddRange(elements);
            if (this.currentSelection == null && this.menuItemElements.Count > 0)
            {
                this.currentSelection = this.menuItemElements[0];
            }

            foreach (var element in elements)
            {
                element.SelectionContext = this;
            }
        }

        public bool IsItemSelected(ISelectable selectableItem)
        {
            Debug.WriteLine("Selected Condition {0}", this.currentSelection == null);
            return selectableItem == this.currentSelection;
        }

        public void Select(ISelectable itemToSelect)
        {
            AbstractMenuItemElement menuItemToSelect = itemToSelect as AbstractMenuItemElement;
            if (menuItemToSelect != null && this.menuItemElements.Contains(menuItemToSelect))
            {
                this.currentSelection = menuItemToSelect;
            }
        }
    }
}