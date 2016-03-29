using SAHL.UI.Halo.Models.Client.PersonalLoan;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Client.PersonalLoan
{
    public class PersonalLoanChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PersonalLoanChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as PersonalLoanChildModel;
            if (model == null) { return; }
            this.HeaderText = model.AccountNumber;
        }
    }
}