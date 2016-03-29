using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
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
    public class when_creating_x2_case_with_errors : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static ISystemMessageCollection systemMessages;
        private static ISystemMessage nonCriticalErrorMessage;
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

            systemMessages = SystemMessageCollection.Empty();
            nonCriticalErrorMessage = new SystemMessage("Adding address failed for client", SystemMessageSeverityEnum.Error);
            systemMessages.AddMessage(nonCriticalErrorMessage);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            applicationStateMachine.WhenToldTo(sm => sm.SystemMessages).Return(systemMessages);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata);
        };

        private It should_create_the_X2_case = () =>
        {
            x2WorkflowManager.WasToldTo(x => x.CreateWorkflowCase(applicationNumber, Param<DomainProcessServiceRequestMetadata>
                .Matches(m =>
                ((DomainProcessServiceRequestMetadata)m).ContainsKey(CoreGlobals.DomainProcessIdName) &&
                ((DomainProcessServiceRequestMetadata)m)[CoreGlobals.DomainProcessIdName] == domainProcess.DomainProcessId.ToString())
            ));
        };

        private It should_fire_the_x2_creation_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, Param.IsAny<Guid>()));
        };
    }
}