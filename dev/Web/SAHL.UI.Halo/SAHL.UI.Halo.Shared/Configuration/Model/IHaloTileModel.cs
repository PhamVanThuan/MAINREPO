using SAHL.Core.Data;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileModel : IDataModel
    {
    }

    public interface IHaloTileModel<T> where T : IHaloTileModel
    {
    }
}
