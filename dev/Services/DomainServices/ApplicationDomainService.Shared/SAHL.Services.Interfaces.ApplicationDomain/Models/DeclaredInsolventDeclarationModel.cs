using SAHL.Core.BusinessModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class DeclaredInsolventDeclarationModel
    {
        public DeclaredInsolventDeclarationModel(OfferDeclarationAnswer answer, DateTime? dateRehabilitated)
        {
            this.Answer = answer;
            this.DateRehabilitated = dateRehabilitated;
        }

        [Required]
        public OfferDeclarationAnswer Answer { get; protected set; }

        public DateTime? DateRehabilitated { get; protected set; }
    }
}