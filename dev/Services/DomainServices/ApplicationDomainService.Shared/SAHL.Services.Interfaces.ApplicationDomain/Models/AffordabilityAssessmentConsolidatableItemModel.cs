using SAHL.Core.BusinessModel.Enums;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityAssessmentConsolidatableItemModel : AffordabilityAssessmentItemModel
    {
        public AffordabilityAssessmentConsolidatableItemModel(AffordabilityAssessmentItemType affordabilityAssessmentItemType)
            : base(affordabilityAssessmentItemType)
        {
        }

        public AffordabilityAssessmentConsolidatableItemModel(int key, 
                                                              int affordabilityAssessmentKey, 
                                                              AffordabilityAssessmentItemType affordabilityAssessmentItemType, 
                                                              DateTime modifiedDate, 
                                                              int modifiedByUserId, 
                                                              int clientValue, 
                                                              int creditValue, 
                                                              int consolidationValue, 
                                                              string comment)
            : base(key, affordabilityAssessmentKey, affordabilityAssessmentItemType, modifiedDate, modifiedByUserId, clientValue, creditValue, comment)
        {
            ConsolidationValue = consolidationValue;
        }

        public int? ConsolidationValue { get; set; }

        public int ToBeValue
        {
            get
            {
                return (CreditValue ?? 0) - (ConsolidationValue ?? 0);
            }
        }
    }
}