using SAHL.Core.UI.Elements.Menus;
using SAHL.Core.UI.Elements.Parts;
using SAHL.Core.UI.Modules;

namespace SAHL.Core.UI.Configuration.Menus
{
    public class StaticSearchRibbonMenuItemConfiguration : AbstractMenuItemConfiguration, IStaticRibbonItemConfiguration
    {
        public StaticSearchRibbonMenuItemConfiguration(IApplicationModule applicationModule, int sequence, string controller, string controllerAction, string requiredFeatureAccess)
            : base(sequence, controller, controllerAction, requiredFeatureAccess)
        {
            this.Sequence = sequence;
            this.StaticText = "Search";
            this.ApplicationModule = applicationModule;
        }

        public IApplicationModule ApplicationModule { get; protected set; }

        public string StaticText { get; protected set; }

        public StaticRibbonMenuItemElement CreateElement()
        {
            var ribbonMenuItem = new StaticRibbonMenuItemElement(string.Format("{0}_search", this.ApplicationModule.Name.ToLower()), this.Url.GenerateActionUrl());
            ribbonMenuItem.AddPart(new IconPart("search"));
            ribbonMenuItem.AddPart(new StaticTextPart(this.StaticText));
            return ribbonMenuItem;
        }
    }
}