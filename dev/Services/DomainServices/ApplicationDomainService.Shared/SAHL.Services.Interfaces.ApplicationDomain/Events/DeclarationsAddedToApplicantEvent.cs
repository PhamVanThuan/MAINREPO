using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class DeclarationsAddedToApplicantEvent : Event
    {
        public DeclarationsAddedToApplicantEvent(DateTime date, int clientKey, int applicationNumber, DateTime captureDate,
            OfferDeclarationAnswer declaredInsolventDeclarationAnswer, DateTime? dateRehabilitated, OfferDeclarationAnswer underAdministrationOrderDeclarationAnswer,
            DateTime? dateAdministrationOrderRescinded, OfferDeclarationAnswer currentlyUnderDebtReviewDeclarationAnswer, bool? hasCurrentDebtArrangement,
            OfferDeclarationAnswer PermissiontoConductCreditCheckDeclarationAnswer)
            : base(date)
        {
            this.ClientKey = clientKey;
            this.ApplicationNumber = applicationNumber;
            this.CaptureDate = captureDate;
            this.DeclaredInsolventDeclarationAnswer = declaredInsolventDeclarationAnswer;
            this.DateRehabilitated = dateRehabilitated;
            this.UnderAdministrationOrderDeclarationAnswer = underAdministrationOrderDeclarationAnswer;
            this.DateAdministrationOrderRescinded = dateAdministrationOrderRescinded;
            this.CurrentlyUnderDebtReviewDeclarationAnswer = currentlyUnderDebtReviewDeclarationAnswer;
            this.HasCurrentDebtArrangement = hasCurrentDebtArrangement;
            this.PermissiontoConductCreditCheckDeclarationAnswer = PermissiontoConductCreditCheckDeclarationAnswer;
        }

        public int ClientKey { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public DateTime CaptureDate { get; protected set; }

        public OfferDeclarationAnswer DeclaredInsolventDeclarationAnswer { get; protected set; }

        public DateTime? DateRehabilitated { get; protected set; }

        public OfferDeclarationAnswer UnderAdministrationOrderDeclarationAnswer { get; protected set; }

        public DateTime? DateAdministrationOrderRescinded { get; protected set; }

        public OfferDeclarationAnswer CurrentlyUnderDebtReviewDeclarationAnswer { get; protected set; }

        public bool? HasCurrentDebtArrangement { get; set; }

        public OfferDeclarationAnswer PermissiontoConductCreditCheckDeclarationAnswer { get; protected set; }
    }
}