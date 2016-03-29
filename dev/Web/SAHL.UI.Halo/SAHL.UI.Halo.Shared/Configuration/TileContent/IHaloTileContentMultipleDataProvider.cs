using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileContentMultipleDataProvider
    {
        IEnumerable<dynamic> Load(BusinessContext businessContext);
    }

    public interface IHaloTileContentMultipleDataProvider<T> : IHaloTileContentMultipleDataProvider where T : IHaloTileModel
    {
    }
}
