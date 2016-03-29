using System;

using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Life
{
    public class LifeChildTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<LifeChildTileHeaderConfiguration>
    {
        public LifeChildTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as LifeChildModel;
            if (model == null) { return; }
            this.HeaderIcons.Add(string.Format("icon-acc-life-{0}", model.AccountStatus).ToLower());
        }
    }
}
