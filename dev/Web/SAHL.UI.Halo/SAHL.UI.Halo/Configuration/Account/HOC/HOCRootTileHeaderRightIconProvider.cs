using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Configuration.Account.HOC
{
    public class HOCRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<HOCRootTileHeaderConfiguration>
    {
        public HOCRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as HOCRootModel;
            if (model == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderIcons.Add(string.Format("icon-acc-hoc-{0}", model.AccountStatus).ToLower());
            if (model.OriginationSourceKey == OriginationSource.SAHomeLoans)
            {
                this.HeaderIcons.Add("icon-originationsource_sahl");
            }
        }
    }
}