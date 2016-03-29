using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;

namespace SAHL.UI.Halo.Shared.Configuration.Header
{
    public abstract class HaloTileHeaderTextProviderBase<T> : IHaloTileHeaderTextProvider<T> 
        where T : IHaloTileHeader
    {
        public string HeaderText { get; protected set; }

        public abstract void Execute<TDataModel>(TDataModel dataModel) where TDataModel : IDataModel;
    }
}
