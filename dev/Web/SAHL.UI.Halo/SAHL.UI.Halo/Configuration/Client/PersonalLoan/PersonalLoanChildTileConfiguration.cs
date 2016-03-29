using SAHL.UI.Halo.Models.Client.PersonalLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.PersonalLoan
{
    public class PersonalLoanChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                      IHaloTileModel<PersonalLoanChildModel>
    {
        public PersonalLoanChildTileConfiguration()
            : base("Personal Loan", "PersonalLoan", 3, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}