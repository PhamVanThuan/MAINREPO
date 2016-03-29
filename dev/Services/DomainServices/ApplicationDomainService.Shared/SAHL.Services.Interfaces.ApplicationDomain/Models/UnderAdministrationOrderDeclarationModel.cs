using SAHL.Core.BusinessModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class UnderAdministrationOrderDeclarationModel
    {
        public UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer answer, DateTime? dateAdministrationOrderRescinded)
        {
            this.Answer = answer;
            this.DateAdministrationOrderRescinded = dateAdministrationOrderRescinded;
        }

        [Required]
        public OfferDeclarationAnswer Answer { get; protected set; }

        public DateTime? DateAdministrationOrderRescinded { get; protected set; }
    }
}