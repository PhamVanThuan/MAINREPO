using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan
{
    public class MortgageLoanRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<MortgageLoanRootTileHeaderConfiguration>
    {
        public MortgageLoanRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as MortgageLoanRootModel;
            if (model == null) { return; }

            this.HeaderIcons.Add(string.Format("icon-acc-mortgage-{0}", model.AccountStatus).ToLower());
            if (model.OriginationSourceKey == OriginationSource.SAHomeLoans)
            {
                this.HeaderIcons.Add("icon-originationsource_sahl");
            }
        }
    }
}