using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityAssessmentLegalEntityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AffordabilityAssessmentLegalEntityDataModel(int affordabilityAssessmentKey, int legalEntityKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public AffordabilityAssessmentLegalEntityDataModel(int affordabilityAssessmentLegalEntityKey, int affordabilityAssessmentKey, int legalEntityKey)
        {
            this.AffordabilityAssessmentLegalEntityKey = affordabilityAssessmentLegalEntityKey;
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int AffordabilityAssessmentLegalEntityKey { get; set; }

        public int AffordabilityAssessmentKey { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.AffordabilityAssessmentLegalEntityKey =  key;
        }
    }
}