using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileHeaderProvider
    {
        void Execute<TDataModel>(TDataModel dataModel) where TDataModel : IDataModel;
    }
}
