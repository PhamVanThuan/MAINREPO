using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Models
{
    public class RateConfigurationValuesModel : ValidatableModel
    {
        public RateConfigurationValuesModel(int RateConfigurationKey, decimal MarginValue, decimal MarketRateValue)
        {
            this.RateConfigurationKey = RateConfigurationKey;
            this.MarketRateValue = MarketRateValue;
            this.MarginValue = MarginValue;
            Validate();
        }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "RateConfigurationKey must be greater than 0.")]
        public int RateConfigurationKey { get; protected set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "MarketRateValue must be greater than 0.")]
        public decimal MarketRateValue { get; protected set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "MarginValue must be greater than 0.")]
        public decimal MarginValue { get; protected set; }
        
    }
}
