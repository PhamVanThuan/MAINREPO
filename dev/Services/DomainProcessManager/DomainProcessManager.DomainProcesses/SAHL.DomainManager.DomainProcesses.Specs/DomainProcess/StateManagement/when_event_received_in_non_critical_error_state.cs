using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_event_received_in_non_critical_error_state : WithFakes
    {
        private static int applicationNumber, employmentKey;
        private static ISystemMessageCollection systemMessages;
        private static ApplicationStateMachine applicationStateMachine;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            systemMessages.AddMessage(new SystemMessage("Invalid bank supplied.", SystemMessageSeverityEnum.Error));

            applicationNumber = 1324;
            employmentKey = 8438;

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.BasicApplicationCreatedTrigger, Guid.NewGuid(), applicationNumber);
            applicationStateMachine.Machine.Fire(applicationStateMachine.ApplicantAddedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.EmploymentAddedTrigger, Guid.NewGuid(), employmentKey);
            applicationStateMachine.Machine.Fire(applicationStateMachine.ApplicationEmploymentDeterminedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.ApplicationHouseHoldIncomeDeterminedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.ApplicationPricedTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.ApplicationFundedTrigger, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.Machine.Fire(applicationStateMachine.NonCriticalErrorOccuredTrigger, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(applicationStateMachine.AddressCapturedTrigger, Guid.NewGuid());
        };

        private It should_continue_to_transition_when_events_recieved = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.NonCriticalErrorOccured).ShouldBeFalse();
        };
    }
}