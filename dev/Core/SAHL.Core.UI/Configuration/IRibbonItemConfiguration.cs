using SAHL.Core.UI.Elements.Menus;

namespace SAHL.Core.UI.Configuration
{
    public interface IRibbonItemConfiguration : IElementWithUrlConfiguration, IElementWthOrderConfiguration
    {
    }

    public interface IStaticRibbonItemConfiguration : IRibbonItemConfiguration, IElementConfiguration<StaticRibbonMenuItemElement>
    {
        string StaticText { get; }
    }

    public interface IDynamicRibbonItemConfiguration : IRibbonItemConfiguration, IBusinessElementConfiguration<DynamicRibbonMenuItemElement>
    {
    }

    public interface IRibbonItemConfiguration<T> : IRibbonItemConfiguration where T : IMenuItemConfiguration
    {
    }
}