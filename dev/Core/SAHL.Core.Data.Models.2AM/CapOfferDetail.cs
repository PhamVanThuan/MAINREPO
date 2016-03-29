using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapOfferDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapOfferDetailDataModel(int capOfferKey, int capTypeConfigurationDetailKey, double effectiveRate, double payment, double fee, int capStatusKey, DateTime? acceptanceDate, int? capNTUReasonKey, DateTime? capNTUReasonDate, DateTime? changeDate, string userID)
        {
            this.CapOfferKey = capOfferKey;
            this.CapTypeConfigurationDetailKey = capTypeConfigurationDetailKey;
            this.EffectiveRate = effectiveRate;
            this.Payment = payment;
            this.Fee = fee;
            this.CapStatusKey = capStatusKey;
            this.AcceptanceDate = acceptanceDate;
            this.CapNTUReasonKey = capNTUReasonKey;
            this.CapNTUReasonDate = capNTUReasonDate;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public CapOfferDetailDataModel(int capOfferDetailKey, int capOfferKey, int capTypeConfigurationDetailKey, double effectiveRate, double payment, double fee, int capStatusKey, DateTime? acceptanceDate, int? capNTUReasonKey, DateTime? capNTUReasonDate, DateTime? changeDate, string userID)
        {
            this.CapOfferDetailKey = capOfferDetailKey;
            this.CapOfferKey = capOfferKey;
            this.CapTypeConfigurationDetailKey = capTypeConfigurationDetailKey;
            this.EffectiveRate = effectiveRate;
            this.Payment = payment;
            this.Fee = fee;
            this.CapStatusKey = capStatusKey;
            this.AcceptanceDate = acceptanceDate;
            this.CapNTUReasonKey = capNTUReasonKey;
            this.CapNTUReasonDate = capNTUReasonDate;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }		

        public int CapOfferDetailKey { get; set; }

        public int CapOfferKey { get; set; }

        public int CapTypeConfigurationDetailKey { get; set; }

        public double EffectiveRate { get; set; }

        public double Payment { get; set; }

        public double Fee { get; set; }

        public int CapStatusKey { get; set; }

        public DateTime? AcceptanceDate { get; set; }

        public int? CapNTUReasonKey { get; set; }

        public DateTime? CapNTUReasonDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public void SetKey(int key)
        {
            this.CapOfferDetailKey =  key;
        }
    }
}