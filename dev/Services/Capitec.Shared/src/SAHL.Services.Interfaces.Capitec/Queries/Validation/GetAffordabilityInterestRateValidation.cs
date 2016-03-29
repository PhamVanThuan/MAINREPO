using SAHL.Core.Services.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Queries.Validation
{
    public class GetAffordabilityInterestRateValidation : IValidation
    {
        [Range(0.01, 1, ErrorMessage = "Interest Rate must be between 1 and 100 percent")]
        [Required]
        public decimal CalcRate { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Income must be greater than 0")] 
        [Required]
        public decimal HouseholdIncome { get; set; }
    }
}
