using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileHeader
    {
    }

    public interface IHaloTileHeader<T> : IHaloTileHeader where T : IHaloTileConfiguration
    {
    }
}
