using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_all_addresses_captured : WithFakes
    {
        static IApplicationStateMachine applicationStateMachine;
        static int applicationNumber;

        Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, Guid.NewGuid());

        };

        Because of = () =>
        {
            applicationStateMachine.Machine.Fire(ApplicationStateTransitionTrigger.AllAddressesCaptured);
        };

        It should_transition_to_all_addresses_captured_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.AllAddressesCaptured).ShouldBeTrue();
        };

    }
}
