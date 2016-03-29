using SAHL.UI.Halo.Configuration.Account.PersonalLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.PersonalLoan
{
    public class PersonalLoanChildTileDrilldown : HaloTileActionDrilldownBase<PersonalLoanChildTileConfiguration, PersonalLoanRootTileConfiguration>,
                                                  IHaloTileActionDrilldown<PersonalLoanChildTileConfiguration>
    {
        public PersonalLoanChildTileDrilldown()
            : base("Personal Loan")
        {
        }
    }
}