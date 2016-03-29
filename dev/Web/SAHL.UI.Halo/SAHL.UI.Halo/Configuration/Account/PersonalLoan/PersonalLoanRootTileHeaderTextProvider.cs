using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan
{
    public class PersonalLoanRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PersonalLoanRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as PersonalLoanRootModel;
            if (model == null) { return; }
            this.HeaderText = model.AccountNumber;
        }
    }
}
