using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class UnemployedEmploymentModel : EmploymentModel
    {
        [Required]
        public UnemployedRemunerationType RemunerationType { get; protected set; }

        public UnemployedEmploymentModel(double basicIncome, int salaryPaymentDay, EmployerModel employer, DateTime startDate, UnemployedRemunerationType remunerationType, 
            EmploymentStatus employmentStatus)
            : base(employer, startDate,  employmentStatus, salaryPaymentDay, basicIncome)
        {
            this.RemunerationType = remunerationType;
            base.EmploymentType = EmploymentType.Unemployed; 
        }
    }
}