using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityAssessmentStatusDataModel :  IDataModel
    {
        public AffordabilityAssessmentStatusDataModel(int affordabilityAssessmentStatusKey, string description)
        {
            this.AffordabilityAssessmentStatusKey = affordabilityAssessmentStatusKey;
            this.Description = description;
		
        }		

        public int AffordabilityAssessmentStatusKey { get; set; }

        public string Description { get; set; }
    }
}