namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloChildTileConfiguration : IHaloSubTileConfiguration
    {
    }

    public interface IHaloChildTileConfiguration<T> : IHaloChildTileConfiguration  where T : IHaloRootTileConfiguration
    {
    }
}
