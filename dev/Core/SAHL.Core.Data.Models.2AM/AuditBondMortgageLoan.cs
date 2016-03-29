using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditBondMortgageLoanDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditBondMortgageLoanDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int bondMortgageLoanKey, int? financialServiceKey, int? bondKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.BondMortgageLoanKey = bondMortgageLoanKey;
            this.FinancialServiceKey = financialServiceKey;
            this.BondKey = bondKey;
		
        }
		[JsonConstructor]
        public AuditBondMortgageLoanDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int bondMortgageLoanKey, int? financialServiceKey, int? bondKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.BondMortgageLoanKey = bondMortgageLoanKey;
            this.FinancialServiceKey = financialServiceKey;
            this.BondKey = bondKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int BondMortgageLoanKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public int? BondKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}