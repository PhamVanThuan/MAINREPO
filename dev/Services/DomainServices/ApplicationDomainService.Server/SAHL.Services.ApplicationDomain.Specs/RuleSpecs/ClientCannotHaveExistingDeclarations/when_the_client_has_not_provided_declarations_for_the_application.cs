using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ClientCannotHaveExistingDeclarations
{
    public class when_the_client_has_not_provided_declarations_for_the_application : WithFakes
    {
        private static ClientCannotHaveExistingDeclarationsRule rule;
        private static IApplicantDataManager applicantDataManager;
        private static ApplicantDeclarationsModel model;
        private static DeclaredInsolventDeclarationModel declaredInsolventDeclaration;
        private static UnderAdministrationOrderDeclarationModel underAdmnistration;
        private static CurrentlyUnderDebtCounsellingReviewDeclarationModel underDebtReview;
        private static PermissionToConductCreditCheckDeclarationModel permissionToConductCreditCheckDeclarationModel;
        private static IEnumerable<OfferDeclarationDataModel> existingDeclarations;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            applicantDataManager = An<IApplicantDataManager>();
            rule = new ClientCannotHaveExistingDeclarationsRule(applicantDataManager);
            declaredInsolventDeclaration = new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null);
            underAdmnistration = new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, null);
            underDebtReview = new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, false);
            permissionToConductCreditCheckDeclarationModel = new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.No);
            model = new ApplicantDeclarationsModel(100, 1010, DateTime.Now, declaredInsolventDeclaration, underAdmnistration,
                underDebtReview, permissionToConductCreditCheckDeclarationModel);
            existingDeclarations = Enumerable.Empty<OfferDeclarationDataModel>();
            applicantDataManager.WhenToldTo(x => x.GetApplicantDeclarations(model.ApplicationNumber, model.ClientKey)).Return(existingDeclarations);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}