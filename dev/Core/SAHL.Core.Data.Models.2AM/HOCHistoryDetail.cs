using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCHistoryDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HOCHistoryDetailDataModel(int hOCHistoryKey, DateTime effectiveDate, string updateType, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, double? hOCProrataPremium, double? hOCMonthlyPremium, DateTime? printDate, DateTime? sendFileDate, DateTime? changeDate, string userID, double hOCAdministrationFee, double hOCBasePremium, double sASRIAAmount, int? hOCRatesKey)
        {
            this.HOCHistoryKey = hOCHistoryKey;
            this.EffectiveDate = effectiveDate;
            this.UpdateType = updateType;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.HOCProrataPremium = hOCProrataPremium;
            this.HOCMonthlyPremium = hOCMonthlyPremium;
            this.PrintDate = printDate;
            this.SendFileDate = sendFileDate;
            this.ChangeDate = changeDate;
            this.UserID = userID;
            this.HOCAdministrationFee = hOCAdministrationFee;
            this.HOCBasePremium = hOCBasePremium;
            this.SASRIAAmount = sASRIAAmount;
            this.HOCRatesKey = hOCRatesKey;
		
        }
		[JsonConstructor]
        public HOCHistoryDetailDataModel(int hOCHistoryDetailKey, int hOCHistoryKey, DateTime effectiveDate, string updateType, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, double? hOCProrataPremium, double? hOCMonthlyPremium, DateTime? printDate, DateTime? sendFileDate, DateTime? changeDate, string userID, double hOCAdministrationFee, double hOCBasePremium, double sASRIAAmount, int? hOCRatesKey)
        {
            this.HOCHistoryDetailKey = hOCHistoryDetailKey;
            this.HOCHistoryKey = hOCHistoryKey;
            this.EffectiveDate = effectiveDate;
            this.UpdateType = updateType;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.HOCProrataPremium = hOCProrataPremium;
            this.HOCMonthlyPremium = hOCMonthlyPremium;
            this.PrintDate = printDate;
            this.SendFileDate = sendFileDate;
            this.ChangeDate = changeDate;
            this.UserID = userID;
            this.HOCAdministrationFee = hOCAdministrationFee;
            this.HOCBasePremium = hOCBasePremium;
            this.SASRIAAmount = sASRIAAmount;
            this.HOCRatesKey = hOCRatesKey;
		
        }		

        public int HOCHistoryDetailKey { get; set; }

        public int HOCHistoryKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string UpdateType { get; set; }

        public double? HOCThatchAmount { get; set; }

        public double? HOCConventionalAmount { get; set; }

        public double? HOCShingleAmount { get; set; }

        public double? HOCProrataPremium { get; set; }

        public double? HOCMonthlyPremium { get; set; }

        public DateTime? PrintDate { get; set; }

        public DateTime? SendFileDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public double HOCAdministrationFee { get; set; }

        public double HOCBasePremium { get; set; }

        public double SASRIAAmount { get; set; }

        public int? HOCRatesKey { get; set; }

        public void SetKey(int key)
        {
            this.HOCHistoryDetailKey =  key;
        }
    }
}