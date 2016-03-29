using System.Collections.Generic;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileDynamicActionProvider
    {
        IEnumerable<IHaloTileAction> GetTileActions(BusinessContext businessContext);
    }

    public interface IHaloTileDynamicActionProvider<T> where T : IHaloTileDynamicActionProvider
    {
    }
}
