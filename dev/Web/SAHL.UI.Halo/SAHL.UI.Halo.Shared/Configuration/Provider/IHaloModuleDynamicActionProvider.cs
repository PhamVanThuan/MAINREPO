namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloModuleDynamicActionProvider<T> : IHaloTileDynamicActionProvider 
        where T : IHaloModuleConfiguration
    {
    }
}
