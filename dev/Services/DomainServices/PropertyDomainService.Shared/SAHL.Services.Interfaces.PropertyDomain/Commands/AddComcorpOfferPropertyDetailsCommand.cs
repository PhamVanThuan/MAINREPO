using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.PropertyDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.PropertyDomain.Commands
{
    public class AddComcorpOfferPropertyDetailsCommand : ServiceCommand, IPropertyDomainCommand, IRequiresOpenApplication
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid Application Number.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public ComcorpOfferPropertyDetailsModel ComcorpOfferPropertyDetails { get; protected set; }

        public AddComcorpOfferPropertyDetailsCommand(int applicationNumber, ComcorpOfferPropertyDetailsModel comcorpOfferPropertyDetails)
        {
            this.ApplicationNumber = applicationNumber;
            this.ComcorpOfferPropertyDetails = comcorpOfferPropertyDetails;
        }
    }
}