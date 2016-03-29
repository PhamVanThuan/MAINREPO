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
    public class when_non_critical_path_error_occurs : WithFakes
    {
        private static int applicationNumber, employmentKey;

        private static ISystemMessageCollection systemMessages;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static ApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            systemMessages.AddMessage(new SystemMessage("Invalid bank supplied.", SystemMessageSeverityEnum.Error));

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationNumber = 1324;
            employmentKey = 32784;

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
        };

        private It should_transition_to_non_critical_error_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.NonCriticalErrorOccured).ShouldBeTrue();
        };

        private It should_adjust_appropriate_state_expectations_frequency = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.NonCriticalErrorOccured).ShouldBeTrue();
        };
    }
}