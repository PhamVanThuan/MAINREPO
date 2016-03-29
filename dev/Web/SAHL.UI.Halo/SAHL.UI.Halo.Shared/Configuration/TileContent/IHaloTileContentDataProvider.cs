using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileContentDataProvider
    {
        dynamic Load(BusinessContext businessContext);
    }

    public interface IHaloTileContentDataProvider<T> : IHaloTileContentDataProvider where T : IHaloTileModel
    {
    }
}
