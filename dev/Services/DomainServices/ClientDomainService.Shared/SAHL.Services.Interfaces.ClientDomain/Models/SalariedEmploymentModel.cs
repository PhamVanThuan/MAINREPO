using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class SalariedEmploymentModel : EmploymentModel, IEmployeeDeductions
    {
        [Required]
        public SalariedRemunerationType RemunerationType { get; protected set; }

        public IEnumerable<EmployeeDeductionModel> EmployeeDeductions { get; protected set; }

        public SalariedEmploymentModel(double basicIncome, int salaryPaymentDay, EmployerModel employer, DateTime startDate, SalariedRemunerationType remunerationType,
            EmploymentStatus employmentStatus, IEnumerable<EmployeeDeductionModel> employeeDeductions)
            : base(employer, startDate,  employmentStatus, salaryPaymentDay, basicIncome)
        {
            this.RemunerationType = remunerationType;
            base.EmploymentType = EmploymentType.Salaried;
            this.EmployeeDeductions = employeeDeductions;
        }
    }
}