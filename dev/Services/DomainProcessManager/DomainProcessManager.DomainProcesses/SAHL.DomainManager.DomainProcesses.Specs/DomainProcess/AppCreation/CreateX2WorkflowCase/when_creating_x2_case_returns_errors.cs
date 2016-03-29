using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.CreateX2WorkflowCase
{
    public class when_creating_x2_case_returns_errors : WithNewPurchaseDomainProcess
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

            domainProcess.ProcessState = applicationStateMachine;
            applicationStateMachine.WhenToldTo(x => x.HasProcessCompletedWithCriticalPathFullyCaptured()).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            systemMessages.AddMessage(new SystemMessage("X2 error!", SystemMessageSeverityEnum.Error));
            x2WorkflowManager.WhenToldTo(x2Manager => x2Manager.CreateWorkflowCase(Param.IsAny<int>(), Param.IsAny<DomainProcessServiceRequestMetadata>())).Return(systemMessages);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata);
        };

        private It should_create_the_X2_case = () =>
        {
            x2WorkflowManager.WasToldTo(x => x.CreateWorkflowCase(applicationNumber, Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_not_fire_the_x2_creation_trigger = () =>
        {
            applicationStateMachine.WasNotToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, Param.IsAny<Guid>()));
        };

        private It should_send_support_email_with_error_message = () =>
        {
            communicationManager.WasToldTo(
                  cm => cm.SendX2CaseCreationFailedSupportEmail(Param.IsAny<ISystemMessageCollection>()
                , Param.IsAny<Guid>()
                , applicationNumber
            ));
        };
    }
}