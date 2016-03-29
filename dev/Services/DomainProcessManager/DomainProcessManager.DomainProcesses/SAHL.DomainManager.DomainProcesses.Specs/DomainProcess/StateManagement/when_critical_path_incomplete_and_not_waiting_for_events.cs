using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

using System;


namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_critical_path_incomplete_and_not_waiting_for_events : WithFakes
    {
        private static ApplicationStateMachine applicationStateMachine;
        private static NewPurchaseApplicationCreationModel creationModel;
        private static bool processingComplete;

        private Establish context = () =>
        {
            creationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            applicationStateMachine = new ApplicationStateMachine();
            applicationStateMachine.CreateStateMachine(creationModel, Guid.NewGuid());
            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), 0);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            //critical state path not complete
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
            processingComplete = applicationStateMachine.HasProcessCompletedWithCriticalPathFullyCaptured();
        };

        private It should_set_the_state_of_processing_complete_to_false = () =>
        {
            processingComplete.ShouldEqual(false);
        };
    }
}