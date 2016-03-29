using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapTypeConfigurationDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapTypeConfigurationDetailDataModel(int capTypeConfigurationKey, int capTypeKey, double rate, int generalStatusKey, double premium, double feePremium, double feeAdmin, double rateFinance, DateTime? changeDate, string userID)
        {
            this.CapTypeConfigurationKey = capTypeConfigurationKey;
            this.CapTypeKey = capTypeKey;
            this.Rate = rate;
            this.GeneralStatusKey = generalStatusKey;
            this.Premium = premium;
            this.FeePremium = feePremium;
            this.FeeAdmin = feeAdmin;
            this.RateFinance = rateFinance;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public CapTypeConfigurationDetailDataModel(int capTypeConfigurationDetailKey, int capTypeConfigurationKey, int capTypeKey, double rate, int generalStatusKey, double premium, double feePremium, double feeAdmin, double rateFinance, DateTime? changeDate, string userID)
        {
            this.CapTypeConfigurationDetailKey = capTypeConfigurationDetailKey;
            this.CapTypeConfigurationKey = capTypeConfigurationKey;
            this.CapTypeKey = capTypeKey;
            this.Rate = rate;
            this.GeneralStatusKey = generalStatusKey;
            this.Premium = premium;
            this.FeePremium = feePremium;
            this.FeeAdmin = feeAdmin;
            this.RateFinance = rateFinance;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }		

        public int CapTypeConfigurationDetailKey { get; set; }

        public int CapTypeConfigurationKey { get; set; }

        public int CapTypeKey { get; set; }

        public double Rate { get; set; }

        public int GeneralStatusKey { get; set; }

        public double Premium { get; set; }

        public double FeePremium { get; set; }

        public double FeeAdmin { get; set; }

        public double RateFinance { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public void SetKey(int key)
        {
            this.CapTypeConfigurationDetailKey =  key;
        }
    }
}