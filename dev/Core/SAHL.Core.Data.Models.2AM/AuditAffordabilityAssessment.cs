using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAffordabilityAssessmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAffordabilityAssessmentDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int affordabilityAssessmentKey, int genericKey, int genericKeyTypeKey, int affordabilityAssessmentStatusKey, int generalStatusKey, int affordabilityAssessmentStressFactorKey, DateTime modifiedDate, int modifiedByUserId, int numberOfContributingApplicants, int numberOfHouseholdDependants, int? minimumMonthlyFixedExpenses, DateTime? confirmedDate, string notes)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.AffordabilityAssessmentStatusKey = affordabilityAssessmentStatusKey;
            this.GeneralStatusKey = generalStatusKey;
            this.AffordabilityAssessmentStressFactorKey = affordabilityAssessmentStressFactorKey;
            this.ModifiedDate = modifiedDate;
            this.ModifiedByUserId = modifiedByUserId;
            this.NumberOfContributingApplicants = numberOfContributingApplicants;
            this.NumberOfHouseholdDependants = numberOfHouseholdDependants;
            this.MinimumMonthlyFixedExpenses = minimumMonthlyFixedExpenses;
            this.ConfirmedDate = confirmedDate;
            this.Notes = notes;
		
        }
		[JsonConstructor]
        public AuditAffordabilityAssessmentDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int affordabilityAssessmentKey, int genericKey, int genericKeyTypeKey, int affordabilityAssessmentStatusKey, int generalStatusKey, int affordabilityAssessmentStressFactorKey, DateTime modifiedDate, int modifiedByUserId, int numberOfContributingApplicants, int numberOfHouseholdDependants, int? minimumMonthlyFixedExpenses, DateTime? confirmedDate, string notes)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.AffordabilityAssessmentStatusKey = affordabilityAssessmentStatusKey;
            this.GeneralStatusKey = generalStatusKey;
            this.AffordabilityAssessmentStressFactorKey = affordabilityAssessmentStressFactorKey;
            this.ModifiedDate = modifiedDate;
            this.ModifiedByUserId = modifiedByUserId;
            this.NumberOfContributingApplicants = numberOfContributingApplicants;
            this.NumberOfHouseholdDependants = numberOfHouseholdDependants;
            this.MinimumMonthlyFixedExpenses = minimumMonthlyFixedExpenses;
            this.ConfirmedDate = confirmedDate;
            this.Notes = notes;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int AffordabilityAssessmentKey { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int AffordabilityAssessmentStatusKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int AffordabilityAssessmentStressFactorKey { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedByUserId { get; set; }

        public int NumberOfContributingApplicants { get; set; }

        public int NumberOfHouseholdDependants { get; set; }

        public int? MinimumMonthlyFixedExpenses { get; set; }

        public DateTime? ConfirmedDate { get; set; }

        public string Notes { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}