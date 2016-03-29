using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileActionDrilldown : IHaloTileAction
    {
        Type RootTileType { get; }
        IHaloRootTileConfiguration RootTileConfiguration { get; }
    }

    public interface IHaloTileActionDrilldown<T> :IHaloTileActionDrilldown  where T : IHaloTileConfiguration
    {
    }
}
