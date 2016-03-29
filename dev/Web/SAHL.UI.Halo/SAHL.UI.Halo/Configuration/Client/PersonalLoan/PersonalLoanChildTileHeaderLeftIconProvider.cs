using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.PersonalLoan
{
    public class PersonalLoanChildTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<PersonalLoanChildTileHeaderConfiguration>
    {
        public PersonalLoanChildTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}