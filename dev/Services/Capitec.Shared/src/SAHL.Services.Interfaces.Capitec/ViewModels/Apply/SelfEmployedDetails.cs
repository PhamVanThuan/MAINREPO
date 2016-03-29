using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class SelfEmployedDetails
    {
        public SelfEmployedDetails(decimal grossMonthlyIncome)
        {
            this.GrossMonthlyIncome = grossMonthlyIncome;
        }

        [Required(ErrorMessage = "Gross Monthly Individual Income is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Gross Monthly Individual Income must be a number.")]
        public decimal GrossMonthlyIncome { get; set; }
    }
}
