using SAHL.UI.Halo.Models.Client.MortgageLoan;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan
{
    public class MortgageLoanChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                      IHaloTileModel<MortgageLoanChildModel>
    {
        public MortgageLoanChildTileConfiguration()
            : base("Mortgage Loan", "MortgageLoan", 1, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}