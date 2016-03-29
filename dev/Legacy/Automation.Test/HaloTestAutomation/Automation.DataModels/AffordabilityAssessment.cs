using Common.Enums;
using System;
using System.Collections.Generic;
namespace Automation.DataModels
{
    public sealed class AffordabilityAssessment : Record, IDataModel
    {
        public int AffordabilityAssessmentKey { get; set; }
        public int GenericKey { get; set; }
        public GenericKeyTypeEnum GenericKeyTypeKey { get; set; }
        public AffordabilityAssessmentStatusKey AffordabilityAssessmentStatusKey { get; set; }
        public GeneralStatusEnum GeneralStatusKey { get; set; }
        public AffordabilityAssessmentStressFactor AffordabilityAssessmentStressFactorKey { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public int NumberOfContributingApplicants { get; set; }
        public int NumberOfHouseholdDependants { get; set; }
        public int MinimumMonthlyFixedExpenses { get; set; }
    }

    public sealed class AffordabilityAssessmentItem
    {
        public int AffordabilityAssessmentItemKey { get; set; }
        public int AffordabilityAssessmentKey { get; set; }
        public int AffordabilityAssessmentItemTypeKey { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public int ClientValue { get; set; }
        public int CreditValue { get; set; }
        public int DebtToConsolidateValue { get; set; }
        public string ItemNotes { get; set; }
        public int AffordabilityAssessmentItemCategoryKey { get; set; }
    }
}