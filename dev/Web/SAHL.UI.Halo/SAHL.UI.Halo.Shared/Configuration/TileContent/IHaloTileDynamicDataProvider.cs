using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileDynamicDataProvider
    {
        HaloDynamicTileDataModel LoadDynamicData(BusinessContext businessContext);
    }

    public interface IHaloTileDynamicDataProvider<T> where T : IHaloTileConfiguration
    {
    }
}
