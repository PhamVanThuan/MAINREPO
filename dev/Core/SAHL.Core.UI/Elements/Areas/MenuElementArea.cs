using Newtonsoft.Json;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Elements.Menus;
using System.Collections.Generic;

namespace SAHL.Core.UI.Elements.Areas
{
    public class MenuElementArea : ElementArea
    {
        [JsonProperty]
        private Dictionary<string, MenuBarRibbonElement> ribbonBars;

        [JsonProperty]
        private Dictionary<string, MenuBarContextElement> contextBars;

        // menu bars
        private MenuBarMenuElement menuBar;

        public MenuElementArea()
            : base(ElementNames.MenuArea)
        {
            this.ribbonBars = new Dictionary<string, MenuBarRibbonElement>();
            this.contextBars = new Dictionary<string, MenuBarContextElement>();

            // Setup the menubar
            this.menuBar = new MenuBarMenuElement();
        }

        public MenuElementArea(Dictionary<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>> menuItemConfiguration)
            : base(ElementNames.MenuArea)
        {
            this.ribbonBars = new Dictionary<string, MenuBarRibbonElement>();
            this.contextBars = new Dictionary<string, MenuBarContextElement>();

            // Setup the menubar
            this.menuBar = new MenuBarMenuElement();

            foreach (KeyValuePair<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>> kvp in menuItemConfiguration)
            {
                var menuItem = kvp.Key.CreateElement();
                this.menuBar.AddMenuElement(menuItem);

                var ribbonItems = kvp.Value;
                var ribbonBar = new MenuBarRibbonElement();
                this.ribbonBars.Add(menuItem.Id, ribbonBar);

                foreach (IStaticRibbonItemConfiguration ribbonItemConfig in ribbonItems)
                {
                    var ribbonItem = ribbonItemConfig.CreateElement();
                    ribbonBar.AddRibbonMenuElement(ribbonItem);

                    var contextItem = new MenuBarContextElement();
                    this.contextBars.Add(ribbonItem.Id, contextItem);
                }
            }
        }

        public MenuBarMenuElement MenuBar
        {
            get { return this.menuBar; }
        }

        [JsonIgnore]
        public MenuBarRibbonElement RibbonBar
        {
            get
            {
                if (this.menuBar.CurrentSelection != null)
                {
                    MenuBarRibbonElement ribbonBar;
                    if (this.ribbonBars.TryGetValue((this.menuBar.CurrentSelection as AbstractMenuItemElement).Id, out ribbonBar))
                    {
                        return ribbonBar;
                    }
                }
                return null;
            }
        }

        [JsonIgnore]
        public MenuBarContextElement ContextBar
        {
            get
            {
                if (this.menuBar.CurrentSelection != null)
                {
                    MenuBarRibbonElement ribbonBar;
                    if (this.ribbonBars.TryGetValue((this.menuBar.CurrentSelection as AbstractMenuItemElement).Id, out ribbonBar))
                    {
                        if (ribbonBar != null)
                        {
                            MenuBarContextElement contextBar;
                            if (ribbonBar.CurrentSelection != null)
                            {
                                if (this.contextBars.TryGetValue((this.menuBar.CurrentSelection as AbstractMenuItemElement).Id, out contextBar))
                                {
                                    return contextBar;
                                }
                                else
                                {
                                    contextBar = new MenuBarContextElement();
                                    this.contextBars.Add((this.menuBar.CurrentSelection as AbstractMenuItemElement).Id, contextBar);
                                    return contextBar;
                                }
                            }
                        }
                    }
                }
                return null;
            }
        }
    }
}