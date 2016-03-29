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
    public class when_populating_applicant_declarations_and_under_debt_review : WithCoreFakes
    {
        private static DeclarationsModelManager modelManager;
        private static Applicant applicant;
        private static List<ApplicantDeclarationsModel> result;

        private Establish context = () =>
        {
            modelManager = new DeclarationsModelManager();
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant, Core.BusinessModel.Enums.MaritalStatus.Single,
                false, false, true, false, true);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateDeclarations(applicant);
        };

        private It should_populate_the_under_debt_counselling_declaration_with_an_answer_of_yes = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey)
                .FirstOrDefault().DeclarationAnswer.ShouldEqual(OfferDeclarationAnswer.Yes);
        };

        private It _with_an_answer_of_yesshould_populate_the_has_debt_arrangement_declaration_with_an_answer_of_yes = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey)
                .FirstOrDefault().DeclarationAnswer.ShouldEqual(OfferDeclarationAnswer.Yes);
        };
    }
}