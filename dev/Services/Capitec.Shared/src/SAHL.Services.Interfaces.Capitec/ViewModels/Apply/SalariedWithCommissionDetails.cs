using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class SalariedWithCommissionDetails
    {
        public SalariedWithCommissionDetails(decimal grossMonthlyIncome, decimal threeMonthAverageCommission)
        {
            this.GrossMonthlyIncome = grossMonthlyIncome;
            this.ThreeMonthAverageCommission = threeMonthAverageCommission;
        }

        [Required(ErrorMessage = "Gross Monthly Individual Income is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Gross Monthly Individual Income must be a number.")]
        public decimal GrossMonthlyIncome { get; set; }

        [Required(ErrorMessage = "3 Month Average Commission is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "3 Month Average Commission must be a number.")]
        public decimal ThreeMonthAverageCommission { get; set; }
    }
}
