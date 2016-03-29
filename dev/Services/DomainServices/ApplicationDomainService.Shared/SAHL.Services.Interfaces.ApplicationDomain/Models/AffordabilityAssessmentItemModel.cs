using SAHL.Core.BusinessModel.Enums;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentItemModel
    {
        public AffordabilityAssessmentItemModel(AffordabilityAssessmentItemType affordabilityAssessmentItemType)
        {
            AffordabilityAssessmentItemType = affordabilityAssessmentItemType;
        }

        public AffordabilityAssessmentItemModel(int key, int affordabilityAssessmentKey, AffordabilityAssessmentItemType affordabilityAssessmentItemType, DateTime modifiedDate, 
                                                int modifiedByUserId, int? clientValue, int? creditValue, string itemNotes)
        {
            Key = key;
            AffordabilityAssessmentKey = affordabilityAssessmentKey;
            AffordabilityAssessmentItemType = affordabilityAssessmentItemType;
            ModifiedDate = modifiedDate;
            ModifiedByUserId = modifiedByUserId;
            ClientValue = clientValue;
            CreditValue = creditValue;
            ItemNotes = ItemNotes;
        }

        public int Key { get; set; }

        public int AffordabilityAssessmentKey { get; set; }

        public AffordabilityAssessmentItemType AffordabilityAssessmentItemType { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedByUserId { get; set; }

        public int? ClientValue { get; set; }

        public int? CreditValue { get; set; }

        public string ItemNotes { get; set; }
    }
}