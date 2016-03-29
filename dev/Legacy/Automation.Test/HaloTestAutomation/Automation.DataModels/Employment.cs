using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class Employment
    {
        public string Employer { get; set; }

        public string EmploymentType { get; set; }

        public string RemunerationType { get; set; }

        public string StartDate { get; set; }

        public int MonthlyIncomeRands { get; set; }

        public string SubsidyProvider { get; set; }

        public string SalaryNumber { get; set; }

        public string PayPoint { get; set; }

        public string Rank { get; set; }

        public string Notch { get; set; }

        public int StopOrderAmount { get; set; }

        public int? SalaryPaymentDay { get; set; }

        public int EmployerKey { get; set; }

        public EmploymentStatusEnum EmploymentStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public DateTime? EmploymentStartDate { get; set; }

        public bool? ConfirmedEmploymentFlag { get; set; }

        public EmploymentConfirmationSourceEnum? EmploymentConfirmationSourceKey { get; set; }

        public bool? ConfirmedIncomeFlag { get; set; }

        public double BasicIncome { get; set; }

        public RemunerationTypeEnum RemunerationTypeKey { get; set; }

        public EmploymentTypeEnum EmploymentTypeKey { get; set; }

        public DateTime? EmploymentEndDate { get; set; }

        public string ContactPerson { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string ContactPhoneCode { get; set; }

        public string ConfirmedBy { get; set; }

        public DateTime? ConfirmedDate { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string Department { get; set; }

        public decimal? Commission { get; set; }

        public decimal? Overtime { get; set; }

        public decimal? Shift { get; set; }

        public decimal? Performance { get; set; }

        public decimal? Allowances { get; set; }

        public decimal? PAYE { get; set; }

        public decimal? PensionProvident { get; set; }

        public decimal? UIF { get; set; }

        public decimal? MedicalAid { get; set; }

        public double? ConfirmedBasicIncome { get; set; }

        public decimal? ConfirmedCommission { get; set; }

        public decimal? ConfirmedOvertime { get; set; }

        public decimal? ConfirmedShift { get; set; }

        public decimal? ConfirmedPerformance { get; set; }

        public decimal? ConfirmedAllowances { get; set; }

        public decimal? ConfirmedPAYE { get; set; }

        public decimal? ConfirmedUIF { get; set; }

        public decimal? ConfirmedPensionProvident { get; set; }

        public decimal? ConfirmedMedicalAid { get; set; }

        public string JobTitle { get; set; }

        public int EmploymentKey { get; set; }

        public bool? UnionMembership { get; set; }
    }
}