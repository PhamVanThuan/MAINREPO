using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Client.Detail.EmploymentSummary
{
    public class EmploymentSummaryChildModel : IHaloTileModel
    {
        public decimal CurrentTotalUnconfirmedIncome { get; set; }

        public decimal CurrentTotalConfirmedIncome { get; set; }

        public string PrimaryEmploymentType { get; set; }
    }
}