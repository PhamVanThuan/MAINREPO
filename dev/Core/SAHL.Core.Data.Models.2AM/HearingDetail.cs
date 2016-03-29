using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HearingDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HearingDetailDataModel(int debtCounsellingKey, int hearingTypeKey, int hearingAppearanceTypeKey, int? courtKey, string caseNumber, DateTime hearingDate, int generalStatusKey, string comment)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
            this.HearingTypeKey = hearingTypeKey;
            this.HearingAppearanceTypeKey = hearingAppearanceTypeKey;
            this.CourtKey = courtKey;
            this.CaseNumber = caseNumber;
            this.HearingDate = hearingDate;
            this.GeneralStatusKey = generalStatusKey;
            this.Comment = comment;
		
        }
		[JsonConstructor]
        public HearingDetailDataModel(int hearingDetailKey, int debtCounsellingKey, int hearingTypeKey, int hearingAppearanceTypeKey, int? courtKey, string caseNumber, DateTime hearingDate, int generalStatusKey, string comment)
        {
            this.HearingDetailKey = hearingDetailKey;
            this.DebtCounsellingKey = debtCounsellingKey;
            this.HearingTypeKey = hearingTypeKey;
            this.HearingAppearanceTypeKey = hearingAppearanceTypeKey;
            this.CourtKey = courtKey;
            this.CaseNumber = caseNumber;
            this.HearingDate = hearingDate;
            this.GeneralStatusKey = generalStatusKey;
            this.Comment = comment;
		
        }		

        public int HearingDetailKey { get; set; }

        public int DebtCounsellingKey { get; set; }

        public int HearingTypeKey { get; set; }

        public int HearingAppearanceTypeKey { get; set; }

        public int? CourtKey { get; set; }

        public string CaseNumber { get; set; }

        public DateTime HearingDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public string Comment { get; set; }

        public void SetKey(int key)
        {
            this.HearingDetailKey =  key;
        }
    }
}