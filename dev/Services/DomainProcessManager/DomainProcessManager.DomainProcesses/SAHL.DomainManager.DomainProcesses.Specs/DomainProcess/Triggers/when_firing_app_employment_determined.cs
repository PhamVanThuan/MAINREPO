using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_app_employment_determined : WithFakes
    {
        static IApplicationStateMachine applicationStateMachine;
        static int applicationNumber;
        static List<ApplicationStateTransitionTrigger> permittedTriggers;

        Establish context = () =>
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
        };

        Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
        };

        It should_transition_to_application_employment_determined_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.ApplicationEmploymentDetermined).ShouldBeTrue();
        };
    }
}
