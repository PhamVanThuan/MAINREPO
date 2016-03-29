using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class CAP2_OffersDataModel :  IDataModel
    {
        public CAP2_OffersDataModel(long instanceID, string capBroker, int? capOfferKey, int? capOfferDetailKey, string legalEntityName, int? accountKey, int? capNTUReasonKey, int? promotion, DateTime? capExpireDate, string capCreditBroker, int? capPaymentOptionKey, int? capStatusKey, int? genericKey, string last_State)
        {
            this.InstanceID = instanceID;
            this.CapBroker = capBroker;
            this.CapOfferKey = capOfferKey;
            this.CapOfferDetailKey = capOfferDetailKey;
            this.LegalEntityName = legalEntityName;
            this.AccountKey = accountKey;
            this.CapNTUReasonKey = capNTUReasonKey;
            this.Promotion = promotion;
            this.CapExpireDate = capExpireDate;
            this.CapCreditBroker = capCreditBroker;
            this.CapPaymentOptionKey = capPaymentOptionKey;
            this.CapStatusKey = capStatusKey;
            this.GenericKey = genericKey;
            this.Last_State = last_State;
		
        }		

        public long InstanceID { get; set; }

        public string CapBroker { get; set; }

        public int? CapOfferKey { get; set; }

        public int? CapOfferDetailKey { get; set; }

        public string LegalEntityName { get; set; }

        public int? AccountKey { get; set; }

        public int? CapNTUReasonKey { get; set; }

        public int? Promotion { get; set; }

        public DateTime? CapExpireDate { get; set; }

        public string CapCreditBroker { get; set; }

        public int? CapPaymentOptionKey { get; set; }

        public int? CapStatusKey { get; set; }

        public int? GenericKey { get; set; }

        public string Last_State { get; set; }
    }
}