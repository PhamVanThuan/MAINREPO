using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class EmploymentModel : ValidatableModel
    {
        protected EmploymentModel(EmployerModel employer, DateTime startDate, EmploymentStatus employmentStatus, int salaryPaymentDay, double basicIncome)
        {
            this.Employer = employer;
            this.StartDate = startDate;
            this.EmploymentStatus = employmentStatus;
            this.SalaryPaymentDay = salaryPaymentDay;
            this.BasicIncome = basicIncome;
            Validate();
        }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage="Basic Income must be provided.")]
        public double BasicIncome { get; protected set; }

        [Required]
        public EmployerModel Employer { get; protected set; }

        [Required]
        public DateTime StartDate { get; protected set; }

        [Required]
        public EmploymentType EmploymentType { get; protected set; }

        [Required]
        public EmploymentStatus EmploymentStatus { get; protected set; }

        [Range(0, 31)]
        public int SalaryPaymentDay { get; protected set; }
    }
}