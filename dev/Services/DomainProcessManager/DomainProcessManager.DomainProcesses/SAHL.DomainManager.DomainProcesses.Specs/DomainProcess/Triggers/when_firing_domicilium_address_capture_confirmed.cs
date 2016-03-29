using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_domicilium_address_capture_confirmed : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(ApplicationStateTransitionTrigger.AllAddressesCaptured);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, Guid.NewGuid());
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, Guid.NewGuid());
        };

        private It should_transition_to_domicilium_address_captured_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.DomiciliumAddressCaptured).ShouldBeTrue();
        };
    }
}