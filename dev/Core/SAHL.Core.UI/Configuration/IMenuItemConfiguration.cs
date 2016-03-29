using SAHL.Core.UI.Elements.Menus;
using SAHL.Core.UI.Modules;

namespace SAHL.Core.UI.Configuration
{
    public interface IMenuItemConfiguration : IElementConfiguration<StaticMenuItemElement>, IElementWithUrlConfiguration, IElementWthOrderConfiguration, IRequiredFeatureAccess
    {
        string StaticText { get; }
    }

    public interface IMenuItemConfiguration<T> : IMenuItemConfiguration where T : IApplicationModule
    {
    }
}