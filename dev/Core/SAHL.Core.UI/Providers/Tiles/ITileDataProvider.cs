using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;
using System.Collections.Generic;

namespace SAHL.Core.UI.Providers.Tiles
{
    public interface ITileDataProvider
    {
        IEnumerable<BusinessKey> GetTileInstanceKeys(BusinessKey businessKey);
    }

    public interface ITileDataProvider<T> : ITileDataProvider where T : ITileModel
    {
    }
}