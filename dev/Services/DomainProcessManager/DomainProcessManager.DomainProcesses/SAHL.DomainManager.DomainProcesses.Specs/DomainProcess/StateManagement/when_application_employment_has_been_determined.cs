using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_application_employment_has_been_determined : WithFakes
    {
        private static ApplicationStateMachine applicationStateMachine;
        private static NewPurchaseApplicationCreationModel creationModel;
        private static int applicationNumber = 1;

        private Establish context = () =>
        {
            creationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(creationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
        };

        private It should_go_into_the_application_employment_determined_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.ApplicationEmploymentDetermined).ShouldBeTrue();
        };
    }
}