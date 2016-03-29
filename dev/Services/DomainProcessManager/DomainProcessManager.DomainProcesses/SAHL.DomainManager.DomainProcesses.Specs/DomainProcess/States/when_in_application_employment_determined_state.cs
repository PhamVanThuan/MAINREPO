using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.States
{
    public class when_in_application_employment_determined_state : WithFakes
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
                ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed,
                ApplicationStateTransitionTrigger.ApplicationPricingConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported,
            };

            applicationStateMachine.CreateStateMachine(applicationCreationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), applicationNumber);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
        };

        private It should_be_in_application_employment_determined_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.ApplicationEmploymentDetermined).ShouldBeTrue();
        };

        private It should_only_permit_the_permitted_triggers = () =>
        {
            applicationStateMachine.Machine.PermittedTriggers.ShouldContainOnly(permittedTriggers);
        };
    }
}
