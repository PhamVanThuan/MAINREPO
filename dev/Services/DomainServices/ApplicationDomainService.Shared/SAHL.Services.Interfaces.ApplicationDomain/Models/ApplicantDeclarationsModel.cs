using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicantDeclarationsModel : ValidatableModel
    {
        public ApplicantDeclarationsModel(int clientKey, int applicationNumber, DateTime captureDate,
            DeclaredInsolventDeclarationModel declaredInsolventDeclaration,
            UnderAdministrationOrderDeclarationModel underAdministrationOrderDeclaration,
            CurrentlyUnderDebtCounsellingReviewDeclarationModel currentlyUnderDebtReviewDeclaration,
            PermissionToConductCreditCheckDeclarationModel permissiontoConductCreditCheckDeclaration)
        {
            this.ClientKey = clientKey;
            this.ApplicationNumber = applicationNumber;
            this.CaptureDate = captureDate;
            this.DeclaredInsolventDeclaration = declaredInsolventDeclaration;
            this.UnderAdministrationOrderDeclaration = underAdministrationOrderDeclaration;
            this.CurrentlyUnderDebtReviewDeclaration = currentlyUnderDebtReviewDeclaration;
            this.PermissiontoConductCreditCheckDeclaration = permissiontoConductCreditCheckDeclaration;
            Validate();
        }
        
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage="A ClientKey must be provided.")]
        public int ClientKey { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "An Application Number must be provided.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public DateTime CaptureDate { get; protected set; }

        [Required]
        public DeclaredInsolventDeclarationModel DeclaredInsolventDeclaration { get; protected set; }

        [Required]
        public UnderAdministrationOrderDeclarationModel UnderAdministrationOrderDeclaration { get; protected set; }

        [Required]
        public CurrentlyUnderDebtCounsellingReviewDeclarationModel CurrentlyUnderDebtReviewDeclaration { get; protected set; }

        [Required]
        public PermissionToConductCreditCheckDeclarationModel PermissiontoConductCreditCheckDeclaration { get; protected set; }
    }
}
