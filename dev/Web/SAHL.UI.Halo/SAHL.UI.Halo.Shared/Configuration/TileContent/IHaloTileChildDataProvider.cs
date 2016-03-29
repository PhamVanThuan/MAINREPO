using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileChildDataProvider : IHaloTileDataProvider
    {
        IEnumerable<BusinessContext> LoadSubKeys(BusinessContext businessContext);
    }

    public interface IHaloTileChildDataProvider<T> where T : IHaloTileConfiguration
    {
    }
}
