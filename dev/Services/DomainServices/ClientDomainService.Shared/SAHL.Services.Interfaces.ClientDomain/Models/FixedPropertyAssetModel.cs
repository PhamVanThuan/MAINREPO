using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class FixedPropertyAssetModel : ValidatableModel
    {
        public FixedPropertyAssetModel(DateTime dateAquired, int addressKey, double assetValue, double liabilityValue)
        {
            this.DateAquired = dateAquired;
            this.AddressKey = addressKey;
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
            Validate();
        }

        [Required]
        public DateTime DateAquired { get; protected set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Address must be provided.")]
        public int AddressKey { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Asset value must be provided.")]
        public double AssetValue { get; protected set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Liability value must be provided.")]
        public double LiabilityValue { get; protected set; }
    }
}
