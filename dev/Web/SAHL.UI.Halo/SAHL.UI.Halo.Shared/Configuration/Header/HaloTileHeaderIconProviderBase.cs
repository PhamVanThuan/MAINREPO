using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileHeaderIconProviderBase<T> : IHaloTileHeaderIconProvider<T> 
        where T : IHaloTileHeader
    {
        protected HaloTileHeaderIconProviderBase(HaloTileIconAlignment iconAlignment)
        {
            this.IconAlignment = iconAlignment;
            this.HeaderIcons   = new List<string>();
        }

        public HaloTileIconAlignment IconAlignment { get; protected set; }
        public IList<string> HeaderIcons { get; protected set; }

        public abstract void Execute<TDataModel>(TDataModel dataModel) where TDataModel : IDataModel;
    }
}
