using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicantAffordabilityModel : ValidatableModel
    {
        public ApplicantAffordabilityModel(IEnumerable<AffordabilityTypeModel> clientAffordabilityAssessment, int clientKey, int applicationNumber)
        {
            this.ClientKey = clientKey;
            this.ApplicationNumber = applicationNumber;
            this.ClientAffordabilityAssessment = clientAffordabilityAssessment;

            Validate();
        }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ClientKey must be provided.")]
        public int ClientKey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "An Application Number must be provided.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public IEnumerable<AffordabilityTypeModel> ClientAffordabilityAssessment { get; protected set; }
    }
}