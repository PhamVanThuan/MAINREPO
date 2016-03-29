using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Configuration.Account.Life
{
    public class LifeRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<LifeRootTileHeaderConfiguration>
    {
        public LifeRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as LifeRootModel;
            if (model == null) { return; }
            this.HeaderIcons.Add(string.Format("icon-acc-life-{0}", model.AccountStatus).ToLower());
        }
    }
}
