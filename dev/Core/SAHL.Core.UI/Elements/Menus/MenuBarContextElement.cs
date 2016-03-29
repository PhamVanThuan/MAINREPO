using Newtonsoft.Json;
using System.Collections.Generic;

namespace SAHL.Core.UI.Elements.Menus
{
    public class MenuBarContextElement : VisualElement
    {
        [JsonProperty]
        private List<AbstractContextMenuItemElement> contextMenuItemElements;

        public MenuBarContextElement()
            : base("menubar-context")
        {
            this.contextMenuItemElements = new List<AbstractContextMenuItemElement>();
        }

        public IEnumerable<AbstractContextMenuItemElement> MenuItemElements
        {
            get { return this.contextMenuItemElements; }
        }

        public void AddContextMenuElement(AbstractContextMenuItemElement element)
        {
            this.contextMenuItemElements.Add(element);
        }

        public void AddContextMenuElementRange(IEnumerable<AbstractContextMenuItemElement> elements)
        {
            this.contextMenuItemElements.AddRange(elements);
        }

        public void ClearItems()
        {
            this.contextMenuItemElements.Clear();
        }

        public void RemoveItemsAfterMenuElement(AbstractContextMenuItemElement elementToRemoveAfter)
        {
            int index = this.contextMenuItemElements.IndexOf(elementToRemoveAfter);
            if (this.contextMenuItemElements.Count > 1 && index >= 0 && index < this.contextMenuItemElements.Count - 1)
            {
                this.contextMenuItemElements.RemoveRange(index + 1, this.contextMenuItemElements.Count - index - 1);
            }
        }
    }
}