using SAHL.UI.Halo.Models.Client.HOC;
using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan.HOC
{
    public class HOCTileHeaderTextProvider : HaloTileHeaderTextProviderBase<HOCChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var hocChildModel = dataModel as HOCChildModel;
            if (hocChildModel == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderText = hocChildModel.PolicyNumber;
        }
    }
}
