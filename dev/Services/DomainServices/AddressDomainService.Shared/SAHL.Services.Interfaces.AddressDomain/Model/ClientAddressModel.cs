using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Model
{
    public class ClientAddressModel : ValidatableModel
    {
        public ClientAddressModel(int clientKey, int addressKey, AddressType addressType)
        {
            this.ClientKey = clientKey;
            this.AddressKey = addressKey;
            this.AddressType = addressType;
            Validate();
        }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ClientKey must be provided.")]
        public int ClientKey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "An AddressKey must be provided.")]
        public int AddressKey { get; protected set; }

        [Required]
        public AddressType AddressType { get; protected set; }
    }
}