
namespace SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration
{
    public interface IHaloRootTileLinkedConfiguration
    {
        string Name { get; }
    }

    public interface IHaloRootTileLinkedConfiguration<T> : IHaloRootTileLinkedConfiguration where T : IHaloRootTileConfiguration
    {
    }
}
