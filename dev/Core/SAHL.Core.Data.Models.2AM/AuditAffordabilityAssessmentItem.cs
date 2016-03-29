using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAffordabilityAssessmentItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAffordabilityAssessmentItemDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int affordabilityAssessmentItemKey, int affordabilityAssessmentKey, int affordabilityAssessmentItemTypeKey, DateTime modifiedDate, int modifiedByUserId, int? clientValue, int? creditValue, int? debtToConsolidateValue, string itemNotes)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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
		[JsonConstructor]
        public AuditAffordabilityAssessmentItemDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int affordabilityAssessmentItemKey, int affordabilityAssessmentKey, int affordabilityAssessmentItemTypeKey, DateTime modifiedDate, int modifiedByUserId, int? clientValue, int? creditValue, int? debtToConsolidateValue, string itemNotes)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
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

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int AffordabilityAssessmentItemKey { get; set; }

        public int AffordabilityAssessmentKey { get; set; }

        public int AffordabilityAssessmentItemTypeKey { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedByUserId { get; set; }

        public int? ClientValue { get; set; }

        public int? CreditValue { get; set; }

        public int? DebtToConsolidateValue { get; set; }

        public string ItemNotes { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}