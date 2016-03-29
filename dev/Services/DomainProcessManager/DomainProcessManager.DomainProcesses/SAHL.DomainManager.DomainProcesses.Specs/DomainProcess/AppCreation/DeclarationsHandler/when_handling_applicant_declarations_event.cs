using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.DeclarationsHandler
{
    public class when_handling_applicant_declarations_event : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static DeclarationsAddedToApplicantEvent declarationsAddedToApplicantEvent;
        private static SAHL.Services.Interfaces.ApplicationDomain.Models.ApplicantDeclarationsModel applicantDeclarationsModel;
        private static int clientKey, applicationNumber;

        private Establish context = () =>
        {
            clientKey = 123;
            applicationNumber = 313;
            applicantDeclarationsModel = new SAHL.Services.Interfaces.ApplicationDomain.Models.ApplicantDeclarationsModel(1234, 67, DateTime.MinValue,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, DateTime.MinValue),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));

            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);
            declarationsAddedToApplicantEvent = new DeclarationsAddedToApplicantEvent(DateTime.Now, clientKey, applicationNumber, DateTime.Now,
                applicantDeclarationsModel.DeclaredInsolventDeclaration.Answer, applicantDeclarationsModel.DeclaredInsolventDeclaration.DateRehabilitated,
                applicantDeclarationsModel.UnderAdministrationOrderDeclaration.Answer, applicantDeclarationsModel.UnderAdministrationOrderDeclaration.DateAdministrationOrderRescinded,
                applicantDeclarationsModel.CurrentlyUnderDebtReviewDeclaration.Answer, applicantDeclarationsModel.CurrentlyUnderDebtReviewDeclaration.HasCurrentDebtArrangement,
                applicantDeclarationsModel.PermissiontoConductCreditCheckDeclaration.Answer);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(declarationsAddedToApplicantEvent, serviceRequestMetadata);
        };

        private It should_raise_applicant_declarations_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed, declarationsAddedToApplicantEvent.Id));
        };
    }
}