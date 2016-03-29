using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityAssessmentItemTypeDataModel :  IDataModel
    {
        public AffordabilityAssessmentItemTypeDataModel(int affordabilityAssessmentItemTypeKey, int affordabilityAssessmentItemCategoryKey, string description, bool applyStressFactor, bool consolidatable)
        {
            this.AffordabilityAssessmentItemTypeKey = affordabilityAssessmentItemTypeKey;
            this.AffordabilityAssessmentItemCategoryKey = affordabilityAssessmentItemCategoryKey;
            this.Description = description;
            this.ApplyStressFactor = applyStressFactor;
            this.Consolidatable = consolidatable;
		
        }		

        public int AffordabilityAssessmentItemTypeKey { get; set; }

        public int AffordabilityAssessmentItemCategoryKey { get; set; }

        public string Description { get; set; }

        public bool ApplyStressFactor { get; set; }

        public bool Consolidatable { get; set; }
    }
}