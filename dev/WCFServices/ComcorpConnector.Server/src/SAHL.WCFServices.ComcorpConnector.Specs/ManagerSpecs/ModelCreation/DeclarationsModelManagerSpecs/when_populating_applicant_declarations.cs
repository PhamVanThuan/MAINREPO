using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.DeclarationsModelManagerSpecs
{
    public class when_populating_applicant_declarations : WithCoreFakes
    {
        private static DeclarationsModelManager modelManager;
        private static Applicant applicant;
        private static List<ApplicantDeclarationsModel> result;

        private Establish context = () =>
        {
            modelManager = new DeclarationsModelManager();
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant, Core.BusinessModel.Enums.MaritalStatus.Single,
                false, false, false, false, true);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateDeclarations(applicant);
        };

        private It should_contain_an_admin_order_declaration_with_the_answer_set_to_no = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey && x.DeclarationAnswer == OfferDeclarationAnswer.No)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_an_insolvency_declaration_with_the_answer_set_to_no = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey && x.DeclarationAnswer == OfferDeclarationAnswer.No)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_the_debt_counselling_declaration_with_the_answer_set_to_no = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey && x.DeclarationAnswer == OfferDeclarationAnswer.No)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_the_permission_to_conduct_credit_check_declaration_with_the_answer_set_to_yes = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey && x.DeclarationAnswer == OfferDeclarationAnswer.Yes)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };
    }
}