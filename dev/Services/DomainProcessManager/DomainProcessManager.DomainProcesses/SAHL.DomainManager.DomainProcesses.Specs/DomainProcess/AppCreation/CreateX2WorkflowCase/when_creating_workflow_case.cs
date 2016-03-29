using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.CreateX2WorkflowCase
{
    public class when_creating_workflow_case : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static NewBusinessApplicationFundedEvent newBusinessApplicationFundedEvent;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;

        private Establish context = () =>
        {
            applicationNumber = 11432;
            newBusinessApplicationFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 01, 01).Date, applicationNumber);
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(Core.BusinessModel.Enums.OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            // Avoid coupling with methods handling applicants details
            applicationCreationModel.Applicants = new List<ApplicantModel>();
            domainProcess.DataModel = applicationCreationModel;

            applicationStateMachine.WhenToldTo(x => x.HasProcessCompletedWithCriticalPathFullyCaptured()).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata);
        };

        private It should_fire_the_x2_creation_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, Param.IsAny<Guid>()));
        };

        private It should_not_send_cam_team_email_with_error_message = () =>
        {
            communicationManager.WasNotToldTo(
                  cm => cm.SendNonCriticalErrorsEmail(Param.IsAny<ISystemMessageCollection>()
                , applicationNumber
            ));
        };
    }
}