using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditEmployerDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditEmployerDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int employerKey, string name, string telephoneNumber, string telephoneCode, string contactPerson, string contactEmail, string accountantName, string accountantContactPerson, string accountantTelephoneCode, string accountantTelephoneNumber, string accountantEmail, int employerBusinessTypeKey, string userID, DateTime? changeDate, int employmentSectorKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.EmployerKey = employerKey;
            this.Name = name;
            this.TelephoneNumber = telephoneNumber;
            this.TelephoneCode = telephoneCode;
            this.ContactPerson = contactPerson;
            this.ContactEmail = contactEmail;
            this.AccountantName = accountantName;
            this.AccountantContactPerson = accountantContactPerson;
            this.AccountantTelephoneCode = accountantTelephoneCode;
            this.AccountantTelephoneNumber = accountantTelephoneNumber;
            this.AccountantEmail = accountantEmail;
            this.EmployerBusinessTypeKey = employerBusinessTypeKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.EmploymentSectorKey = employmentSectorKey;
		
        }
		[JsonConstructor]
        public AuditEmployerDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int employerKey, string name, string telephoneNumber, string telephoneCode, string contactPerson, string contactEmail, string accountantName, string accountantContactPerson, string accountantTelephoneCode, string accountantTelephoneNumber, string accountantEmail, int employerBusinessTypeKey, string userID, DateTime? changeDate, int employmentSectorKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.EmployerKey = employerKey;
            this.Name = name;
            this.TelephoneNumber = telephoneNumber;
            this.TelephoneCode = telephoneCode;
            this.ContactPerson = contactPerson;
            this.ContactEmail = contactEmail;
            this.AccountantName = accountantName;
            this.AccountantContactPerson = accountantContactPerson;
            this.AccountantTelephoneCode = accountantTelephoneCode;
            this.AccountantTelephoneNumber = accountantTelephoneNumber;
            this.AccountantEmail = accountantEmail;
            this.EmployerBusinessTypeKey = employerBusinessTypeKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.EmploymentSectorKey = employmentSectorKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int EmployerKey { get; set; }

        public string Name { get; set; }

        public string TelephoneNumber { get; set; }

        public string TelephoneCode { get; set; }

        public string ContactPerson { get; set; }

        public string ContactEmail { get; set; }

        public string AccountantName { get; set; }

        public string AccountantContactPerson { get; set; }

        public string AccountantTelephoneCode { get; set; }

        public string AccountantTelephoneNumber { get; set; }

        public string AccountantEmail { get; set; }

        public int EmployerBusinessTypeKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int EmploymentSectorKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}