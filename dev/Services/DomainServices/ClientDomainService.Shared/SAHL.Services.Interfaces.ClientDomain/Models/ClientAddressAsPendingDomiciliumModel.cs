using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class ClientAddressAsPendingDomiciliumModel : ValidatableModel
    {
        public ClientAddressAsPendingDomiciliumModel(int clientAddressKey, int clientKey)
        {
            this.ClientAddresskey = clientAddressKey;
            this.ClientKey = clientKey;
            Validate();
        }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ClientAddressKey must be provided.")]
        public int ClientAddresskey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ClientKey must be provided.")]
        public int ClientKey { get; protected set; }
    }
}
