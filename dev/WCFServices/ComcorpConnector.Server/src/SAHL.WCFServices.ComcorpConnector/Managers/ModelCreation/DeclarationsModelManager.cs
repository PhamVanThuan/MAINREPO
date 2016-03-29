using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class DeclarationsModelManager : IDeclarationsModelManager
    {
        public List<ApplicantDeclarationsModel> PopulateDeclarations(Applicant comcorpApplicant)
        {
            List<ApplicantDeclarationsModel> applicantDeclarations = new List<ApplicantDeclarationsModel>();
            // Have you ever been declared insolvent?
            ApplicantDeclarationsModel applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey, comcorpApplicant.DeclaredInsolvent
                ? OfferDeclarationAnswer.Yes : OfferDeclarationAnswer.No, null);
            applicantDeclarations.Add(applicantDeclaration);
            if (comcorpApplicant.DeclaredInsolvent)
            {
                // If yes, date rehabilitated
                applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey, OfferDeclarationAnswer.Date,
                    comcorpApplicant.DateRehabilitated);
                applicantDeclarations.Add(applicantDeclaration);
            }
            // Have you ever been under administration order?
            applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey, comcorpApplicant.IsUnderAdminOrder
                ? OfferDeclarationAnswer.Yes : OfferDeclarationAnswer.No, null);
            applicantDeclarations.Add(applicantDeclaration);

            if (comcorpApplicant.IsUnderAdminOrder)
            {
                applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.AdministrationOrderDateRescindedQuestionKey, OfferDeclarationAnswer.Date,
                comcorpApplicant.AdminRescinded);
                applicantDeclarations.Add(applicantDeclaration);
            }
            // Are you currently under debt counselling or review in terms of the National Credit Act, 2005?
            applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey, comcorpApplicant.IsUnderDebtReview ?
                OfferDeclarationAnswer.Yes : OfferDeclarationAnswer.No, null);
            applicantDeclarations.Add(applicantDeclaration);
            if (comcorpApplicant.IsUnderDebtReview)
            {
                // If yes, do you currently have a debt rearrangement/s in place?
                applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey, comcorpApplicant.DebtRearrangement ?
                    OfferDeclarationAnswer.Yes : OfferDeclarationAnswer.No, null);
                applicantDeclarations.Add(applicantDeclaration);
            }
            // Do you agree to SAHL conducting a credit check?
            applicantDeclaration = new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey, comcorpApplicant.MayDoCreditBureauEnquiry ?
                OfferDeclarationAnswer.Yes : OfferDeclarationAnswer.No, null);
            applicantDeclarations.Add(applicantDeclaration);

            return applicantDeclarations;
        }
    }
}