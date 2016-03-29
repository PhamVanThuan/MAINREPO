using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Shared
{
    public interface ITileDataRepository
    {
        IHaloTileModel FindTileDataModel<T>(T tileConfiguration) where T : class, IHaloTileConfiguration;
        IHaloTileState FindTilePageState<T>(T tileConfiguration) where T : class, IHaloTileConfiguration;
        IHaloTileChildDataProvider FindTileChildDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IHaloTileDynamicDataProvider FindTileDynamicDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IHaloTileContentDataProvider FindTileContentDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IHaloTileContentMultipleDataProvider FindTileContentMultipleDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration;
        IHaloTileEditorDataProvider FindTileEditorDataProvider<T>(T tileConfiguration) where T : IHaloTileConfiguration;
    }
}
