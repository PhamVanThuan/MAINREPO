using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.StateManagement
{
    public class when_critical_path_completed_and_not_waiting_for_events : WithFakes
    {
        private static ApplicationStateMachine applicationStateMachine;
        private static bool processComplete;

        private Establish coxtext = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            var creationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            applicationStateMachine.CreateStateMachine(creationModel, Guid.NewGuid());

            applicationStateMachine.TriggerBasicApplicationCreated(Guid.NewGuid(), 0);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, Guid.NewGuid());
            applicationStateMachine.TriggerEmploymentAdded(Guid.NewGuid(), 123);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, Guid.NewGuid());

            var commandCorrelationGuid = Guid.NewGuid();
            applicationStateMachine.RecordCommandSent(typeof(LinkBankAccountToClientCommand), commandCorrelationGuid);
            applicationStateMachine.RecordEventReceived(commandCorrelationGuid);
        };

        private Because of = () =>
        {
            processComplete = applicationStateMachine.HasProcessCompletedWithCriticalPathFullyCaptured();
        };

        private It should_set_the_state_of_processing_complete_to_true = () =>
        {
            processComplete.ShouldBeTrue();
        };
    }
}