using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_applicant_addition_confirmed1 : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();

            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), 123456);
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
        };

        private It should_transition_to_applicant_added_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.ApplicantAdded).ShouldBeTrue();
        };
    }
}
