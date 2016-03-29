using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_affordability_detail_capture_confirmed : WithFakes
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
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed, Guid.NewGuid());
        };

        private It should_transition_to_affordability_detail_captured_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.AffordabilityDetailCaptured).ShouldBeTrue();
        };
    }
}