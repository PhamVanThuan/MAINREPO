namespace SAHL.Core.UI.Configuration
{
    public interface IDrillDownTileConfiguration
    {
    }

    public interface IDrillDownTileConfiguration<P> : IDrillDownTileConfiguration, IMajorTileConfiguration where P : ITileConfiguration
    {
    }
}