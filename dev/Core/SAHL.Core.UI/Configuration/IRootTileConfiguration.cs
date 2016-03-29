using SAHL.Core.UI.Modules;

namespace SAHL.Core.UI.Configuration
{
    public interface IRootTileConfiguration : IMajorTileConfiguration
    {
    }

    public interface IRootTileConfiguration<T> : IRootTileConfiguration where T : IApplicationModule
    {
    }
}