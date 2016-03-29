using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.PersonalLoan
{
    public class PersonalLoanRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<PersonalLoanRootModel>,
                                                     IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>
    {
        public PersonalLoanRootTileConfiguration()
            : base("Personal Loan", "PersonalLoan", 2, noOfRows: 2)
        {
        }
    }
}