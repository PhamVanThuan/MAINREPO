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
    public class when_populating_applicant_declarations_with_a_rehab_date : WithCoreFakes
    {
        private static DeclarationsModelManager modelManager;
        private static Applicant applicant;
        private static List<ApplicantDeclarationsModel> result;

        private Establish context = () =>
        {
            modelManager = new DeclarationsModelManager();
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant, Core.BusinessModel.Enums.MaritalStatus.Single,
                false, true, false, false, true);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateDeclarations(applicant);
        };

        private It should_populate_the_insolvency_rehabilitation_date_declaration = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_set_the_rehabilitation_date_from_the_comcorp_applicant_model = () =>
        {
            result.Where(x => x.DeclarationQuestion == OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey)
                .FirstOrDefault().DeclarationDate.ShouldEqual(applicant.DateRehabilitated);
        };
    }
}