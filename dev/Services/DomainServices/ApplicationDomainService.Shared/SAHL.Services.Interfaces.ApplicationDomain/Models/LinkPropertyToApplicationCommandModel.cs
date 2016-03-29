using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class LinkPropertyToApplicationCommandModel : ValidatableModel
    {
        public LinkPropertyToApplicationCommandModel(int ApplicationNumber, int PropertyKey)
        {
            this.ApplicationNumber = ApplicationNumber;
            this.PropertyKey = PropertyKey;

            this.Validate();
        }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "An Application Number must be provided.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A Property Key must be provided.")]
        public int PropertyKey { get; protected set; }
    }
}
