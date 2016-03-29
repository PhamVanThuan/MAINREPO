using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Client.DebtCounselling
{
    public class DebtCounsellingChildModel : IHaloTileModel
    {
        public int NumOpenDebtCounsellingCases { get; set; }

        public int NumActiveDebtCounsellingProposals { get; set; }

        public string DebtCounsellor { get; set; }
    }
}