namespace SAHL.Core.UI.Providers.Tiles
{
    public class TileProviders
    {
        public ITileContentProvider ContentProviders { get; protected set; }

        public ITileDataProvider DataProvider { get; protected set; }
    }
}