using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_application_debit_order_capture_confirmed : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            permittedTriggers = new List<ApplicationStateTransitionTrigger> { ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed,
                ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, ApplicationStateTransitionTrigger.CriticalErrorReported };
            applicationStateMachine = new ApplicationStateMachine();
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine = Common.getApplicationStateMachineWithCriticalPathCaptured(1232, applicationStateMachine, applicationCreationModel);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, Guid.NewGuid());
        };

        private It should_transition_state_to_application_debit_order_captured_state = () =>
        {
            applicationStateMachine.IsInState(ApplicationState.ApplicationDebitOrderCaptured).ShouldBeTrue();
        };
    }
}