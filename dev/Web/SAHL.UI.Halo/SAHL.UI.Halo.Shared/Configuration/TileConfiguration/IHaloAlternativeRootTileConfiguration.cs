namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloAlternativeRootTileConfiguration : IHaloRootTileConfiguration
    {
    }

    public interface IHaloAlternativeRootTileConfiguration<T> : IHaloAlternativeRootTileConfiguration where T : IHaloDynamicRootTileConfiguration
    {
    }
}
