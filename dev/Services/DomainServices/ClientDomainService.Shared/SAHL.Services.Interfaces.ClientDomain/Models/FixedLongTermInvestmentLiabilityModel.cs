using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class FixedLongTermInvestmentLiabilityModel : ValidatableModel
    {
        public FixedLongTermInvestmentLiabilityModel(string companyName, double liabilityValue)
        {
            this.CompanyName = companyName;
            this.LiabilityValue = liabilityValue;
            Validate();
        }

        [Required]
        public string CompanyName { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Liability value must be provided.")]
        public double LiabilityValue { get; protected set; }
    }
}