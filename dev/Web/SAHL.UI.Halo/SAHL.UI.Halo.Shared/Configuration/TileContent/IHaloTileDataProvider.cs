using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileDataProvider
    {
        string GetSqlStatement(BusinessContext businessContext);
    }
}
