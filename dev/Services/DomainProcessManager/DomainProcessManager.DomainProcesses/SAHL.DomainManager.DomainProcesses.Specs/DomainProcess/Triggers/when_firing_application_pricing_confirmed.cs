using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_application_pricing_confirmed : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            permittedTriggers = new List<ApplicationStateTransitionTrigger> {
                ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed,
                ApplicationStateTransitionTrigger.ApplicationPricingConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported
            };
            applicationStateMachine = new ApplicationStateMachine();
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), 1231);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, Guid.NewGuid());
        };

        private It should_transition_state_to_application_priced_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.ApplicationPriced).ShouldBeTrue();
        };
    }
}
