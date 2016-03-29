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
    public class when_firing_employment_addition : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static int applicationNumber;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            permittedTriggers = new List<ApplicationStateTransitionTrigger> {
                ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed,
                ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported,
                ApplicationStateTransitionTrigger.EmploymentAdditionConfirmed
            };

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
        };

        private It should_transition_to_employment_added_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.EmploymentAdded).ShouldBeTrue();
        };
    }
}
