using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.Client.PersonalLoan;

namespace SAHL.UI.Halo.Configuration.Client.PersonalLoan
{
    public class PersonalLoanChildTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<PersonalLoanChildTileHeaderConfiguration>
    {
        public PersonalLoanChildTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as PersonalLoanChildModel;
            if (model == null) { return; }
            this.HeaderIcons.Add(string.Format("icon-app-personal-{0}", model.AccountStatus).ToLower());

            if (model.OriginationSource == SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans)
            {
                this.HeaderIcons.Add("icon-originationsource_sahl");
            }
        }
    }
}
