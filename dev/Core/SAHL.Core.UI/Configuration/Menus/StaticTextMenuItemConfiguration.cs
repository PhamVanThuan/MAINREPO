using SAHL.Core.UI.Elements.Menus;
using SAHL.Core.UI.Elements.Parts;
using SAHL.Core.UI.Modules;

namespace SAHL.Core.UI.Configuration.Menus
{
    public abstract class StaticTextMenuItemConfiguration : AbstractMenuItemConfiguration, IMenuItemConfiguration
    {
        public StaticTextMenuItemConfiguration(IApplicationModule applicationModule, string requiredFeatureAccess, int sequence, string staticText, string controller, string controllerAction)
            : base(sequence, controller, controllerAction, requiredFeatureAccess)
        {
            this.StaticText = staticText;
            this.ApplicationModule = applicationModule;
        }

        public IApplicationModule ApplicationModule { get; protected set; }

        public string StaticText { get; protected set; }

        public StaticMenuItemElement CreateElement()
        {
            var menu = new StaticMenuItemElement(this.ApplicationModule.Name.ToLower(), Url.GenerateActionUrl());
            menu.AddPart(new StaticTextPart(this.StaticText));
            return menu;
        }
    }
}