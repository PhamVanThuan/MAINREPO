using SAHL.UI.Halo.Models.Client.HOC;
using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan.HOC
{
    public class HOCTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<HOCChildTileHeaderConfiguration>
    {
        public HOCTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            HOCChildModel hocChildModel = dataModel as HOCChildModel;
            if (hocChildModel == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderIcons.Add(string.Format("icon-acc-hoc-{0}", hocChildModel.HocAccountStatus).ToLower());
            if (hocChildModel.IsSAHLHOC)
            {
                this.HeaderIcons.Add("icon-originationsource_sahl");
            }
        }
    }
}
