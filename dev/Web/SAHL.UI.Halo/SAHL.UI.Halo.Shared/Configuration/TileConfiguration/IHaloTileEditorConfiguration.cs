namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileEditorConfiguration : IHaloTileConfiguration
    {
    }

    public interface IHaloTileEditorConfiguration<T> : IHaloTileEditorConfiguration where T : IHaloTileConfiguration 
    {
    }
}
