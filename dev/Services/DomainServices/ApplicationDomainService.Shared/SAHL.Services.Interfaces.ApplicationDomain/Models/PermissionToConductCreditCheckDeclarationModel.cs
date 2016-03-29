using SAHL.Core.BusinessModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class PermissionToConductCreditCheckDeclarationModel
    {
        public PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer answer)
        {
            this.Answer = answer;
        }

        [Required]
        public OfferDeclarationAnswer Answer { get; protected set; }
    }
}