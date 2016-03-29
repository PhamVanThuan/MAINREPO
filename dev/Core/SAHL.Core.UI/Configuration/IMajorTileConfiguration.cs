using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration
{
    public interface IMajorTileConfiguration : ITileConfiguration, IBusinessElementConfiguration<MajorTileElement>
    {
        string ContextDescription { get; }
    }

    public interface IMajorTileConfiguration<T> : IMajorTileConfiguration where T : ITileModel
    {
    }
}