using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan.HOC
{
    public class HOCTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<HOCChildTileHeaderConfiguration>
    {
        public HOCTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}
