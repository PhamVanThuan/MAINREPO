using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloDynamicRootTileConfiguration : IHaloRootTileConfiguration
    {
        IHaloRootTileConfiguration GetRootTileConfiguration(BusinessContext businessContext);
        IHaloRootTileConfiguration GetRootTileConfiguration(HaloDynamicTileDataModel dynamicTileDataModel);
    }
}
