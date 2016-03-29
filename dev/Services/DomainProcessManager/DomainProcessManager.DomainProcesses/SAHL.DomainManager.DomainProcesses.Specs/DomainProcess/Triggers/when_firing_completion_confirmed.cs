using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    internal class when_firing_completion_confirmed : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, Guid.NewGuid());
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(ApplicationStateTransitionTrigger.AllAddressesCaptured);
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.CompletionConfirmed, Guid.NewGuid());
        };

        private It should_transition_to_completed_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.Completed).ShouldBeTrue();
        };
    }
}