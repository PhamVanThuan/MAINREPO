using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan
{
    public class PersonalLoanRootTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<PersonalLoanRootTileHeaderConfiguration>
    {
        public PersonalLoanRootTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}
