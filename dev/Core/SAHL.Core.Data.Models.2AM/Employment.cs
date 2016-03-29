using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class EmploymentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public EmploymentDataModel(int? employerKey, int employmentTypeKey, int remunerationTypeKey, int employmentStatusKey, int legalEntityKey, DateTime? employmentStartDate, DateTime? employmentEndDate, string contactPerson, string contactPhoneNumber, string contactPhoneCode, string confirmedBy, DateTime? confirmedDate, string userID, DateTime? changeDate, string department, double? basicIncome, double? commission, double? overtime, double? shift, double? performance, double? allowances, double? pAYE, double? uIF, double? pensionProvident, double? medicalAid, double? confirmedBasicIncome, double? confirmedCommission, double? confirmedOvertime, double? confirmedShift, double? confirmedPerformance, double? confirmedAllowances, double? confirmedPAYE, double? confirmedUIF, double? confirmedPensionProvident, double? confirmedMedicalAid, string jobTitle, double monthlyIncome, double confirmedIncome, bool? confirmedEmploymentFlag, bool? confirmedIncomeFlag, int? employmentConfirmationSourceKey, int? salaryPaymentDay, bool? unionMember)
        {
            this.EmployerKey = employerKey;
            this.EmploymentTypeKey = employmentTypeKey;
            this.RemunerationTypeKey = remunerationTypeKey;
            this.EmploymentStatusKey = employmentStatusKey;
            this.LegalEntityKey = legalEntityKey;
            this.EmploymentStartDate = employmentStartDate;
            this.EmploymentEndDate = employmentEndDate;
            this.ContactPerson = contactPerson;
            this.ContactPhoneNumber = contactPhoneNumber;
            this.ContactPhoneCode = contactPhoneCode;
            this.ConfirmedBy = confirmedBy;
            this.ConfirmedDate = confirmedDate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.Department = department;
            this.BasicIncome = basicIncome;
            this.Commission = commission;
            this.Overtime = overtime;
            this.Shift = shift;
            this.Performance = performance;
            this.Allowances = allowances;
            this.PAYE = pAYE;
            this.UIF = uIF;
            this.PensionProvident = pensionProvident;
            this.MedicalAid = medicalAid;
            this.ConfirmedBasicIncome = confirmedBasicIncome;
            this.ConfirmedCommission = confirmedCommission;
            this.ConfirmedOvertime = confirmedOvertime;
            this.ConfirmedShift = confirmedShift;
            this.ConfirmedPerformance = confirmedPerformance;
            this.ConfirmedAllowances = confirmedAllowances;
            this.ConfirmedPAYE = confirmedPAYE;
            this.ConfirmedUIF = confirmedUIF;
            this.ConfirmedPensionProvident = confirmedPensionProvident;
            this.ConfirmedMedicalAid = confirmedMedicalAid;
            this.JobTitle = jobTitle;
            this.MonthlyIncome = monthlyIncome;
            this.ConfirmedIncome = confirmedIncome;
            this.ConfirmedEmploymentFlag = confirmedEmploymentFlag;
            this.ConfirmedIncomeFlag = confirmedIncomeFlag;
            this.EmploymentConfirmationSourceKey = employmentConfirmationSourceKey;
            this.SalaryPaymentDay = salaryPaymentDay;
            this.UnionMember = unionMember;
		
        }
		[JsonConstructor]
        public EmploymentDataModel(int employmentKey, int? employerKey, int employmentTypeKey, int remunerationTypeKey, int employmentStatusKey, int legalEntityKey, DateTime? employmentStartDate, DateTime? employmentEndDate, string contactPerson, string contactPhoneNumber, string contactPhoneCode, string confirmedBy, DateTime? confirmedDate, string userID, DateTime? changeDate, string department, double? basicIncome, double? commission, double? overtime, double? shift, double? performance, double? allowances, double? pAYE, double? uIF, double? pensionProvident, double? medicalAid, double? confirmedBasicIncome, double? confirmedCommission, double? confirmedOvertime, double? confirmedShift, double? confirmedPerformance, double? confirmedAllowances, double? confirmedPAYE, double? confirmedUIF, double? confirmedPensionProvident, double? confirmedMedicalAid, string jobTitle, double monthlyIncome, double confirmedIncome, bool? confirmedEmploymentFlag, bool? confirmedIncomeFlag, int? employmentConfirmationSourceKey, int? salaryPaymentDay, bool? unionMember)
        {
            this.EmploymentKey = employmentKey;
            this.EmployerKey = employerKey;
            this.EmploymentTypeKey = employmentTypeKey;
            this.RemunerationTypeKey = remunerationTypeKey;
            this.EmploymentStatusKey = employmentStatusKey;
            this.LegalEntityKey = legalEntityKey;
            this.EmploymentStartDate = employmentStartDate;
            this.EmploymentEndDate = employmentEndDate;
            this.ContactPerson = contactPerson;
            this.ContactPhoneNumber = contactPhoneNumber;
            this.ContactPhoneCode = contactPhoneCode;
            this.ConfirmedBy = confirmedBy;
            this.ConfirmedDate = confirmedDate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.Department = department;
            this.BasicIncome = basicIncome;
            this.Commission = commission;
            this.Overtime = overtime;
            this.Shift = shift;
            this.Performance = performance;
            this.Allowances = allowances;
            this.PAYE = pAYE;
            this.UIF = uIF;
            this.PensionProvident = pensionProvident;
            this.MedicalAid = medicalAid;
            this.ConfirmedBasicIncome = confirmedBasicIncome;
            this.ConfirmedCommission = confirmedCommission;
            this.ConfirmedOvertime = confirmedOvertime;
            this.ConfirmedShift = confirmedShift;
            this.ConfirmedPerformance = confirmedPerformance;
            this.ConfirmedAllowances = confirmedAllowances;
            this.ConfirmedPAYE = confirmedPAYE;
            this.ConfirmedUIF = confirmedUIF;
            this.ConfirmedPensionProvident = confirmedPensionProvident;
            this.ConfirmedMedicalAid = confirmedMedicalAid;
            this.JobTitle = jobTitle;
            this.MonthlyIncome = monthlyIncome;
            this.ConfirmedIncome = confirmedIncome;
            this.ConfirmedEmploymentFlag = confirmedEmploymentFlag;
            this.ConfirmedIncomeFlag = confirmedIncomeFlag;
            this.EmploymentConfirmationSourceKey = employmentConfirmationSourceKey;
            this.SalaryPaymentDay = salaryPaymentDay;
            this.UnionMember = unionMember;
		
        }		

        public int EmploymentKey { get; set; }

        public int? EmployerKey { get; set; }

        public int EmploymentTypeKey { get; set; }

        public int RemunerationTypeKey { get; set; }

        public int EmploymentStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public DateTime? EmploymentStartDate { get; set; }

        public DateTime? EmploymentEndDate { get; set; }

        public string ContactPerson { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string ContactPhoneCode { get; set; }

        public string ConfirmedBy { get; set; }

        public DateTime? ConfirmedDate { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string Department { get; set; }

        public double? BasicIncome { get; set; }

        public double? Commission { get; set; }

        public double? Overtime { get; set; }

        public double? Shift { get; set; }

        public double? Performance { get; set; }

        public double? Allowances { get; set; }

        public double? PAYE { get; set; }

        public double? UIF { get; set; }

        public double? PensionProvident { get; set; }

        public double? MedicalAid { get; set; }

        public double? ConfirmedBasicIncome { get; set; }

        public double? ConfirmedCommission { get; set; }

        public double? ConfirmedOvertime { get; set; }

        public double? ConfirmedShift { get; set; }

        public double? ConfirmedPerformance { get; set; }

        public double? ConfirmedAllowances { get; set; }

        public double? ConfirmedPAYE { get; set; }

        public double? ConfirmedUIF { get; set; }

        public double? ConfirmedPensionProvident { get; set; }

        public double? ConfirmedMedicalAid { get; set; }

        public string JobTitle { get; set; }

        public double MonthlyIncome { get; set; }

        public double ConfirmedIncome { get; set; }

        public bool? ConfirmedEmploymentFlag { get; set; }

        public bool? ConfirmedIncomeFlag { get; set; }

        public int? EmploymentConfirmationSourceKey { get; set; }

        public int? SalaryPaymentDay { get; set; }

        public bool? UnionMember { get; set; }

        public void SetKey(int key)
        {
            this.EmploymentKey =  key;
        }
    }
}