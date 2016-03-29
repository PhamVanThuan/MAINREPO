using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityAffordabilityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityAffordabilityDataModel(int legalEntityKey, int affordabilityTypeKey, double amount, string description, int offerKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AffordabilityTypeKey = affordabilityTypeKey;
            this.Amount = amount;
            this.Description = description;
            this.OfferKey = offerKey;
		
        }
		[JsonConstructor]
        public LegalEntityAffordabilityDataModel(int legalEntityAffordabilityKey, int legalEntityKey, int affordabilityTypeKey, double amount, string description, int offerKey)
        {
            this.LegalEntityAffordabilityKey = legalEntityAffordabilityKey;
            this.LegalEntityKey = legalEntityKey;
            this.AffordabilityTypeKey = affordabilityTypeKey;
            this.Amount = amount;
            this.Description = description;
            this.OfferKey = offerKey;
		
        }		

        public int LegalEntityAffordabilityKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AffordabilityTypeKey { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public int OfferKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityAffordabilityKey =  key;
        }
    }
}