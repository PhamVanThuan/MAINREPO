using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileAction : IHaloAction
    {
        Type TileConfigurationType { get; }
        IHaloTileConfiguration TileConfiguration { get; }
    }
}
