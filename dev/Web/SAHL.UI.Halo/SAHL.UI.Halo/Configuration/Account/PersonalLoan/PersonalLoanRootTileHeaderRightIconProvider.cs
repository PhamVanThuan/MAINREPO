using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan
{
    public class PersonalLoanRootTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<PersonalLoanRootTileHeaderConfiguration>
    {
        public PersonalLoanRootTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as PersonalLoanRootModel;
            if (model == null) { return; }
            this.HeaderIcons.Add(string.Format("icon-app-personal-{0}", model.AccountStatus).ToLower());

            if (model.OriginationSource == OriginationSource.SAHomeLoans)
            {
                this.HeaderIcons.Add("icon-originationsource_sahl");
            }
        }
    }
}
