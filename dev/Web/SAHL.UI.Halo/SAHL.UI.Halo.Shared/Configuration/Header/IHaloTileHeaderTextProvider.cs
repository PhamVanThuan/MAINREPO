using SAHL.Core.Data;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileHeaderTextProvider : IHaloTileHeaderProvider
    {
        string HeaderText { get; }
    }

    public interface IHaloTileHeaderTextProvider<TTileHeader> : IHaloTileHeaderTextProvider
        where TTileHeader : IHaloTileHeader
    {
    }
}
