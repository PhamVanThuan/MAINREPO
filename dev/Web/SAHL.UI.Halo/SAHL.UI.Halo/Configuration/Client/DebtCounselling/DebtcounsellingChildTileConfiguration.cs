using SAHL.UI.Halo.Models.Client.DebtCounselling;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.DebtCounselling
{
    public class DebtCounsellingChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<ClientRootTileConfiguration>,
                                                      IHaloTileModel<DebtCounsellingChildModel>
    {
        public DebtCounsellingChildTileConfiguration()
            : base("Debt Counselling", "DebtCounselling", 4, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}