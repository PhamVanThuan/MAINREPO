using SAHL.Core.BusinessModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class CurrentlyUnderDebtCounsellingReviewDeclarationModel
    {
        public CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer answer, bool? hasCurrentDebtArrangement)
        {
            this.Answer = answer;
            this.HasCurrentDebtArrangement = hasCurrentDebtArrangement;
        }

        [Required]
        public OfferDeclarationAnswer Answer { get; protected set; }

        public bool? HasCurrentDebtArrangement { get; set; }
    }
}