using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditLoanAgreementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditLoanAgreementDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int loanAgreementKey, DateTime agreementDate, double amount, string userName, int bondKey, DateTime? changeDate)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LoanAgreementKey = loanAgreementKey;
            this.AgreementDate = agreementDate;
            this.Amount = amount;
            this.UserName = userName;
            this.BondKey = bondKey;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public AuditLoanAgreementDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int loanAgreementKey, DateTime agreementDate, double amount, string userName, int bondKey, DateTime? changeDate)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LoanAgreementKey = loanAgreementKey;
            this.AgreementDate = agreementDate;
            this.Amount = amount;
            this.UserName = userName;
            this.BondKey = bondKey;
            this.ChangeDate = changeDate;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int LoanAgreementKey { get; set; }

        public DateTime AgreementDate { get; set; }

        public double Amount { get; set; }

        public string UserName { get; set; }

        public int BondKey { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}