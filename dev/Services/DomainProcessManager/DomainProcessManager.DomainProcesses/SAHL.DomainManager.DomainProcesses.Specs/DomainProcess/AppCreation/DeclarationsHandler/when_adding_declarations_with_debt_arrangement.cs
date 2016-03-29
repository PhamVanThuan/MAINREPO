using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.DeclarationsHandler
{
    public class when_adding_declarations_with_debt_arrangement : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static List<ApplicantDeclarationsModel> applicantDeclarations;
        private static int clientKey, applicationNumber;

        private Establish context = () =>
        {
            clientKey = 15;
            applicationNumber = 105;
            applicantDeclarations = new List<ApplicantDeclarationsModel> {
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey, OfferDeclarationAnswer.No, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey, OfferDeclarationAnswer.No, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey, OfferDeclarationAnswer.Yes, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey, OfferDeclarationAnswer.Yes, null),
                new ApplicantDeclarationsModel(OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey, OfferDeclarationAnswer.Yes, null),
            };

            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            newPurchaseCreationModel.Applicants.First().ApplicantDeclarations = applicantDeclarations;
            domainProcess.ProcessState = applicationStateMachine;
            domainProcess.DataModel = newPurchaseCreationModel;

            var clientCollection = new Dictionary<string, int>();
            clientCollection.Add(domainProcess.DataModel.Applicants.First().IDNumber, clientKey);

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
        };

        private Because of = () =>
        {
            domainProcess.AddDeclarations(applicationStateMachine, domainProcess.DataModel.Applicants);
        };

        private It should_add_applicant_declarations_and_set_debt_arrangement_to_true = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddApplicantDeclarationsCommand>.Matches(m =>
                m.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.Answer == OfferDeclarationAnswer.Yes &&
                m.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.HasCurrentDebtArrangement.Value &&
                m.ApplicantDeclarations.DeclaredInsolventDeclaration.Answer == OfferDeclarationAnswer.No &&
                m.ApplicantDeclarations.UnderAdministrationOrderDeclaration.Answer == OfferDeclarationAnswer.No &&
                m.ApplicantDeclarations.PermissiontoConductCreditCheckDeclaration.Answer == OfferDeclarationAnswer.Yes &&
                m.ApplicationNumber == applicationNumber &&
                m.ClientKey == clientKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}