using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAffordabilityAssessmentLegalEntityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAffordabilityAssessmentLegalEntityDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int affordabilityAssessmentLegalEntityKey, int affordabilityAssessmentKey, int legalEntityKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AffordabilityAssessmentLegalEntityKey = affordabilityAssessmentLegalEntityKey;
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public AuditAffordabilityAssessmentLegalEntityDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int affordabilityAssessmentLegalEntityKey, int affordabilityAssessmentKey, int legalEntityKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AffordabilityAssessmentLegalEntityKey = affordabilityAssessmentLegalEntityKey;
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int AffordabilityAssessmentLegalEntityKey { get; set; }

        public int AffordabilityAssessmentKey { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}