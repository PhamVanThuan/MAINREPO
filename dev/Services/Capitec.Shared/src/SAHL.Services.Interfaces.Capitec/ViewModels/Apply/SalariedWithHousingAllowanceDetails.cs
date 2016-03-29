using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class SalariedWithHousingAllowanceDetails
    {

        public SalariedWithHousingAllowanceDetails(decimal grossMonthlyIncome, decimal housingAllowance)
        {
            this.GrossMonthlyIncome = grossMonthlyIncome;
            this.HousingAllowance = housingAllowance;
        }

        [Required(ErrorMessage = "Gross Monthly Individual Income is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Gross Monthly Individual Income must be a number.")]
        public decimal GrossMonthlyIncome { get; set; }

        [Required(ErrorMessage = "Housing Allowance is required for the selected employment type.")]
        [Range(1, int.MaxValue, ErrorMessage = "Housing Allowance must be a number.")]
        public decimal HousingAllowance { get; set; }
    }
}
