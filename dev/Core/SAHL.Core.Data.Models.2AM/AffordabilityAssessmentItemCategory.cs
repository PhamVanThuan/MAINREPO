using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityAssessmentItemCategoryDataModel :  IDataModel
    {
        public AffordabilityAssessmentItemCategoryDataModel(int affordabilityAssessmentItemCategoryKey, string description)
        {
            this.AffordabilityAssessmentItemCategoryKey = affordabilityAssessmentItemCategoryKey;
            this.Description = description;
		
        }		

        public int AffordabilityAssessmentItemCategoryKey { get; set; }

        public string Description { get; set; }
    }
}