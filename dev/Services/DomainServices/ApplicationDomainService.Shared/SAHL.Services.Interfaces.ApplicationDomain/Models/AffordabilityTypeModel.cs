using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class AffordabilityTypeModel : ValidatableModel
    {
        public AffordabilityTypeModel(AffordabilityType affordabilityType, double amount, string description)
        {
            this.AffordabilityType = affordabilityType;
            this.Amount = amount;
            this.Description = description;
            Validate();
        }

        [Required]
        public AffordabilityType AffordabilityType { get; protected set; }

        [Required]
        [Range(1D, Double.MaxValue, ErrorMessage = "Amount must be greater than R 0")]
        public double Amount { get; protected set; }

        public string Description { get; protected set; }
    }
}