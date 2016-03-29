using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration
{
    public interface IChildTileConfiguration : ITileConfiguration, IBusinessElementConfiguration<ChildTileElement>
    {
    }

    public interface IChildTileConfiguration<T> : IChildTileConfiguration where T : ITileModel
    {
    }
}