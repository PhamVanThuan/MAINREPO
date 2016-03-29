using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportEmploymentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportEmploymentDataModel(string employmentTypeKey, string remunerationTypeKey, string employmentStatusKey, int legalEntityKey, string employerName, string employerContactPerson, string employerPhoneCode, string employerPhoneNumber, DateTime? employmentStartDate, DateTime? employmentEndDate, double? monthlyIncome)
        {
            this.EmploymentTypeKey = employmentTypeKey;
            this.RemunerationTypeKey = remunerationTypeKey;
            this.EmploymentStatusKey = employmentStatusKey;
            this.LegalEntityKey = legalEntityKey;
            this.EmployerName = employerName;
            this.EmployerContactPerson = employerContactPerson;
            this.EmployerPhoneCode = employerPhoneCode;
            this.EmployerPhoneNumber = employerPhoneNumber;
            this.EmploymentStartDate = employmentStartDate;
            this.EmploymentEndDate = employmentEndDate;
            this.MonthlyIncome = monthlyIncome;
		
        }
		[JsonConstructor]
        public ImportEmploymentDataModel(int employmentKey, string employmentTypeKey, string remunerationTypeKey, string employmentStatusKey, int legalEntityKey, string employerName, string employerContactPerson, string employerPhoneCode, string employerPhoneNumber, DateTime? employmentStartDate, DateTime? employmentEndDate, double? monthlyIncome)
        {
            this.EmploymentKey = employmentKey;
            this.EmploymentTypeKey = employmentTypeKey;
            this.RemunerationTypeKey = remunerationTypeKey;
            this.EmploymentStatusKey = employmentStatusKey;
            this.LegalEntityKey = legalEntityKey;
            this.EmployerName = employerName;
            this.EmployerContactPerson = employerContactPerson;
            this.EmployerPhoneCode = employerPhoneCode;
            this.EmployerPhoneNumber = employerPhoneNumber;
            this.EmploymentStartDate = employmentStartDate;
            this.EmploymentEndDate = employmentEndDate;
            this.MonthlyIncome = monthlyIncome;
		
        }		

        public int EmploymentKey { get; set; }

        public string EmploymentTypeKey { get; set; }

        public string RemunerationTypeKey { get; set; }

        public string EmploymentStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string EmployerName { get; set; }

        public string EmployerContactPerson { get; set; }

        public string EmployerPhoneCode { get; set; }

        public string EmployerPhoneNumber { get; set; }

        public DateTime? EmploymentStartDate { get; set; }

        public DateTime? EmploymentEndDate { get; set; }

        public double? MonthlyIncome { get; set; }

        public void SetKey(int key)
        {
            this.EmploymentKey =  key;
        }
    }
}