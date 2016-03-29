using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ClientIsAnApplicantOnApplication
{
    public class when_client_is_an_applicant_on_the_application : WithFakes
    {
        static IApplicantDataManager applicantDataManager;
        static ISystemMessageCollection messages;
        static ClientIsAnApplicantOnApplicationRule rule;
        static ApplicantDeclarationsModel ruleModel;
        static DeclaredInsolventDeclarationModel declaredInsolventDeclaration;
        static UnderAdministrationOrderDeclarationModel underAdmnistration;
        static CurrentlyUnderDebtCounsellingReviewDeclarationModel underDebtReview;
        static PermissionToConductCreditCheckDeclarationModel permissionToConductCreditCheckDeclarationModel;

        Establish context = () =>
        {
            applicantDataManager = An<IApplicantDataManager>();

            declaredInsolventDeclaration = new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null);
            underAdmnistration = new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, null);
            underDebtReview = new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, false);
            permissionToConductCreditCheckDeclarationModel = new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.No);

            ruleModel = new ApplicantDeclarationsModel(100, 1010, DateTime.Now, declaredInsolventDeclaration, underAdmnistration,
                underDebtReview, permissionToConductCreditCheckDeclarationModel);

            messages = SystemMessageCollection.Empty();
            rule = new ClientIsAnApplicantOnApplicationRule(applicantDataManager);

            applicantDataManager.WhenToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(Param.IsAny<int>(), Param.IsAny<int>())).Return(true);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_check_if_the_client_an_applicant_is_an_application = () =>
        {
            applicantDataManager.WasToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(ruleModel.ClientKey, ruleModel.ApplicationNumber));
        };

        It should_not_contain_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}
