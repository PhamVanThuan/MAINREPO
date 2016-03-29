using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicantDomiciliumAddressModel : ValidatableModel
    {
        public ApplicantDomiciliumAddressModel(int ClientDomiciliumKey, int ApplicationNumber, int ClientKey)
        {
            this.ClientKey = ClientKey;
            this.ApplicationNumber = ApplicationNumber;
            this.ClientDomiciliumKey = ClientDomiciliumKey;

            Validate();
        }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ClientKey must be provided.")]
        public int ClientKey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "An ApplicationNumber must be provided.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ClientDomiciliumKey must be provided.")]
        public int ClientDomiciliumKey { get; protected set; }

    }
}
