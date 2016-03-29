using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_address_capture_confirmed_trigger_for_a_collection_excluding_the_last_item : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine);
        };

        private Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, Guid.NewGuid());
        };

        private It should_transition_to_the_address_captured_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.AddressCaptured).ShouldBeTrue();
        };
    }
}