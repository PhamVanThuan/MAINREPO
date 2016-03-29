namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileActionWizard : IHaloTileAction
    {
        IHaloWizardTileConfiguration WizardTileConfiguration { get; }
        string ContextData { get; }
    }

    public interface IHaloTileActionWizard<T> : IHaloTileActionWizard
        where T : IHaloRootTileConfiguration, IHaloChildTileConfiguration
    {
    }
}
