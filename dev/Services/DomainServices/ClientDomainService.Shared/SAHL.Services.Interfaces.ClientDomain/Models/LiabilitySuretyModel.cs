using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class LiabilitySuretyModel : ValidatableModel
    {
        public LiabilitySuretyModel(double assetValue, double liabilityValue, string description)
        {
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
            this.Description = description;
            Validate();
        }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Asset value must be provided.")]
        public double AssetValue { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Liability value must be provided.")]
        public double LiabilityValue { get; protected set; }

        [Required]
        public string Description { get; protected set; }
    }
}