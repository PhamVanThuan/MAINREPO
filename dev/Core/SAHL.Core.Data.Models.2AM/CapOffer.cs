using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapOfferDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapOfferDataModel(int accountKey, int capTypeConfigurationKey, int remainingInstallments, double currentBalance, double currentInstallment, double linkRate, int capStatusKey, DateTime offerDate, bool? promotion, int brokerKey, DateTime? capitalisationDate, DateTime? changeDate, int? cAPPaymentOptionKey, string userID)
        {
            this.AccountKey = accountKey;
            this.CapTypeConfigurationKey = capTypeConfigurationKey;
            this.RemainingInstallments = remainingInstallments;
            this.CurrentBalance = currentBalance;
            this.CurrentInstallment = currentInstallment;
            this.LinkRate = linkRate;
            this.CapStatusKey = capStatusKey;
            this.OfferDate = offerDate;
            this.Promotion = promotion;
            this.BrokerKey = brokerKey;
            this.CapitalisationDate = capitalisationDate;
            this.ChangeDate = changeDate;
            this.CAPPaymentOptionKey = cAPPaymentOptionKey;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public CapOfferDataModel(int capOfferKey, int accountKey, int capTypeConfigurationKey, int remainingInstallments, double currentBalance, double currentInstallment, double linkRate, int capStatusKey, DateTime offerDate, bool? promotion, int brokerKey, DateTime? capitalisationDate, DateTime? changeDate, int? cAPPaymentOptionKey, string userID)
        {
            this.CapOfferKey = capOfferKey;
            this.AccountKey = accountKey;
            this.CapTypeConfigurationKey = capTypeConfigurationKey;
            this.RemainingInstallments = remainingInstallments;
            this.CurrentBalance = currentBalance;
            this.CurrentInstallment = currentInstallment;
            this.LinkRate = linkRate;
            this.CapStatusKey = capStatusKey;
            this.OfferDate = offerDate;
            this.Promotion = promotion;
            this.BrokerKey = brokerKey;
            this.CapitalisationDate = capitalisationDate;
            this.ChangeDate = changeDate;
            this.CAPPaymentOptionKey = cAPPaymentOptionKey;
            this.UserID = userID;
		
        }		

        public int CapOfferKey { get; set; }

        public int AccountKey { get; set; }

        public int CapTypeConfigurationKey { get; set; }

        public int RemainingInstallments { get; set; }

        public double CurrentBalance { get; set; }

        public double CurrentInstallment { get; set; }

        public double LinkRate { get; set; }

        public int CapStatusKey { get; set; }

        public DateTime OfferDate { get; set; }

        public bool? Promotion { get; set; }

        public int BrokerKey { get; set; }

        public DateTime? CapitalisationDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? CAPPaymentOptionKey { get; set; }

        public string UserID { get; set; }

        public void SetKey(int key)
        {
            this.CapOfferKey =  key;
        }
    }
}