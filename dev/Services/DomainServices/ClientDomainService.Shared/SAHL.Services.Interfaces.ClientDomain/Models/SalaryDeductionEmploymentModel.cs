using SAHL.Core.BusinessModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class SalaryDeductionEmploymentModel : EmploymentModel, IEmployeeDeductions
    {
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage="Housing Allowance must be provided.")]
        public double HousingAllowance { get; protected set; }

        public SalaryDeductionRemunerationType RemunerationType { get; protected set; }

        public IEnumerable<EmployeeDeductionModel> EmployeeDeductions { get; protected set; }

        public SalaryDeductionEmploymentModel(double basicIncome, double housingAllowance, int salaryPaymentDay, EmployerModel employer, DateTime startDate,
            SalaryDeductionRemunerationType remunerationType, EmploymentStatus employmentStatus, IEnumerable<EmployeeDeductionModel> employeeDeductions)
            : base(employer, startDate, employmentStatus, salaryPaymentDay, basicIncome)
        {
            this.HousingAllowance = housingAllowance;
            this.RemunerationType = remunerationType;
            base.EmploymentType = EmploymentType.SalariedwithDeduction;
            this.EmployeeDeductions = employeeDeductions;
        }
    }
}