using SAHL.UI.Halo.Models.Client.FinancialProfile;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.FinancialProfile
{
    public class FinancialProfileChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                      IHaloTileModel<FinancialProfileChildModel>
    {
        public FinancialProfileChildTileConfiguration()
            : base("Financial Profile", "FinancialProfile", 5, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}