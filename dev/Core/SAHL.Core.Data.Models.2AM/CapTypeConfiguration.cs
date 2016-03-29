using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CapTypeConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CapTypeConfigurationDataModel(DateTime offerStartDate, DateTime offerEndDate, int generalStatusKey, DateTime capEffectiveDate, DateTime capClosureDate, int resetConfigurationKey, DateTime resetDate, int term, DateTime? changeDate, string userID, double? nACQDiscount)
        {
            this.OfferStartDate = offerStartDate;
            this.OfferEndDate = offerEndDate;
            this.GeneralStatusKey = generalStatusKey;
            this.CapEffectiveDate = capEffectiveDate;
            this.CapClosureDate = capClosureDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.ResetDate = resetDate;
            this.Term = term;
            this.ChangeDate = changeDate;
            this.UserID = userID;
            this.NACQDiscount = nACQDiscount;
		
        }
		[JsonConstructor]
        public CapTypeConfigurationDataModel(int capTypeConfigurationKey, DateTime offerStartDate, DateTime offerEndDate, int generalStatusKey, DateTime capEffectiveDate, DateTime capClosureDate, int resetConfigurationKey, DateTime resetDate, int term, DateTime? changeDate, string userID, double? nACQDiscount)
        {
            this.CapTypeConfigurationKey = capTypeConfigurationKey;
            this.OfferStartDate = offerStartDate;
            this.OfferEndDate = offerEndDate;
            this.GeneralStatusKey = generalStatusKey;
            this.CapEffectiveDate = capEffectiveDate;
            this.CapClosureDate = capClosureDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.ResetDate = resetDate;
            this.Term = term;
            this.ChangeDate = changeDate;
            this.UserID = userID;
            this.NACQDiscount = nACQDiscount;
		
        }		

        public int CapTypeConfigurationKey { get; set; }

        public DateTime OfferStartDate { get; set; }

        public DateTime OfferEndDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime CapEffectiveDate { get; set; }

        public DateTime CapClosureDate { get; set; }

        public int ResetConfigurationKey { get; set; }

        public DateTime ResetDate { get; set; }

        public int Term { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public double? NACQDiscount { get; set; }

        public void SetKey(int key)
        {
            this.CapTypeConfigurationKey =  key;
        }
    }
}