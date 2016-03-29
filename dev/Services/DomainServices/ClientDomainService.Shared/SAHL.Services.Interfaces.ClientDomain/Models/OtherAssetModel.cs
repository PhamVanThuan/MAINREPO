using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class OtherAssetModel : ValidatableModel
    {
        public OtherAssetModel(string description, double assetValue, double liabilityValue)
        {
            this.Description = description;
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
            Validate();
        }

        [Required]
        public string Description { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Asset value must be greater than zero.")]
        public double AssetValue { get; protected set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Liability value must be greater than or equal to zero.")]
        public double LiabilityValue { get; protected set; }
    }
}