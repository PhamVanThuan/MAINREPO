using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityAssessmentItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AffordabilityAssessmentItemDataModel(int affordabilityAssessmentKey, int affordabilityAssessmentItemTypeKey, DateTime modifiedDate, int modifiedByUserId, int? clientValue, int? creditValue, int? debtToConsolidateValue, string itemNotes)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.AffordabilityAssessmentItemTypeKey = affordabilityAssessmentItemTypeKey;
            this.ModifiedDate = modifiedDate;
            this.ModifiedByUserId = modifiedByUserId;
            this.ClientValue = clientValue;
            this.CreditValue = creditValue;
            this.DebtToConsolidateValue = debtToConsolidateValue;
            this.ItemNotes = itemNotes;
		
        }
		[JsonConstructor]
        public AffordabilityAssessmentItemDataModel(int affordabilityAssessmentItemKey, int affordabilityAssessmentKey, int affordabilityAssessmentItemTypeKey, DateTime modifiedDate, int modifiedByUserId, int? clientValue, int? creditValue, int? debtToConsolidateValue, string itemNotes)
        {
            this.AffordabilityAssessmentItemKey = affordabilityAssessmentItemKey;
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.AffordabilityAssessmentItemTypeKey = affordabilityAssessmentItemTypeKey;
            this.ModifiedDate = modifiedDate;
            this.ModifiedByUserId = modifiedByUserId;
            this.ClientValue = clientValue;
            this.CreditValue = creditValue;
            this.DebtToConsolidateValue = debtToConsolidateValue;
            this.ItemNotes = itemNotes;
		
        }		

        public int AffordabilityAssessmentItemKey { get; set; }

        public int AffordabilityAssessmentKey { get; set; }

        public int AffordabilityAssessmentItemTypeKey { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedByUserId { get; set; }

        public int? ClientValue { get; set; }

        public int? CreditValue { get; set; }

        public int? DebtToConsolidateValue { get; set; }

        public string ItemNotes { get; set; }

        public void SetKey(int key)
        {
            this.AffordabilityAssessmentItemKey =  key;
        }
    }
}