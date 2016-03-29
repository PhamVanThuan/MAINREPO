using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public enum HaloTileIconAlignment
    {
        Left,
        Right
    }

    public interface IHaloTileHeaderIconProvider : IHaloTileHeaderProvider
    {
        HaloTileIconAlignment IconAlignment { get; }
        IList<string> HeaderIcons { get; }
    }

    public interface IHaloTileHeaderIconProvider<TTileHeader> : IHaloTileHeaderIconProvider
        where TTileHeader : IHaloTileHeader
    {
    }
}
