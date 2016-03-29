using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Client.ITC
{
    public class ITCChildModel : IHaloTileModel
    {
        public string ITCAge { get; set; }

        public decimal CreditScore { get; set; }

        public string CreditScoreVersion { get; set; }

        public string Judgements { get; set; }

        public string Notices { get; set; }

        public string Defaults { get; set; }

        public string TraceAlerts { get; set; }
    }
}