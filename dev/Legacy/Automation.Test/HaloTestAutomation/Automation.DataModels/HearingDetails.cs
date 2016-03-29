using System;

namespace Automation.DataModels
{
    public class HearingDetails
    {
        public int HearingDetailKey { get; set; }

        public int DebtCounsellingKey { get; set; }

        public int HearingTypeKey { get; set; }

        public int HearingAppearanceTypeKey { get; set; }

        public int CourtKey { get; set; }

        public string CaseNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}