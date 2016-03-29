using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditSubsidyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditSubsidyDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int subsidyKey, int subsidyProviderKey, int employmentKey, int legalEntityKey, string salaryNumber, string paypoint, string notch, string rank, int generalStatusKey, double stopOrderAmount, bool gEPFMember)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.SubsidyKey = subsidyKey;
            this.SubsidyProviderKey = subsidyProviderKey;
            this.EmploymentKey = employmentKey;
            this.LegalEntityKey = legalEntityKey;
            this.SalaryNumber = salaryNumber;
            this.Paypoint = paypoint;
            this.Notch = notch;
            this.Rank = rank;
            this.GeneralStatusKey = generalStatusKey;
            this.StopOrderAmount = stopOrderAmount;
            this.GEPFMember = gEPFMember;
		
        }
		[JsonConstructor]
        public AuditSubsidyDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int subsidyKey, int subsidyProviderKey, int employmentKey, int legalEntityKey, string salaryNumber, string paypoint, string notch, string rank, int generalStatusKey, double stopOrderAmount, bool gEPFMember)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.SubsidyKey = subsidyKey;
            this.SubsidyProviderKey = subsidyProviderKey;
            this.EmploymentKey = employmentKey;
            this.LegalEntityKey = legalEntityKey;
            this.SalaryNumber = salaryNumber;
            this.Paypoint = paypoint;
            this.Notch = notch;
            this.Rank = rank;
            this.GeneralStatusKey = generalStatusKey;
            this.StopOrderAmount = stopOrderAmount;
            this.GEPFMember = gEPFMember;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int SubsidyKey { get; set; }

        public int SubsidyProviderKey { get; set; }

        public int EmploymentKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string SalaryNumber { get; set; }

        public string Paypoint { get; set; }

        public string Notch { get; set; }

        public string Rank { get; set; }

        public int GeneralStatusKey { get; set; }

        public double StopOrderAmount { get; set; }

        public bool GEPFMember { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}