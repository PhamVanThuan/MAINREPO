using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Triggers
{
    public class when_firing_x2_case_created_confirmed : WithFakes
    {
        static IApplicationStateMachine applicationStateMachine;
        static int applicationNumber;

        Establish context = () =>
        {
            applicationStateMachine = new ApplicationStateMachine();
            applicationNumber = 123456;

            Common.getApplicationStateMachineWithCriticalPathCaptured(applicationNumber, applicationStateMachine);
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, Guid.NewGuid());
            applicationStateMachine.Machine.Fire(ApplicationStateTransitionTrigger.AllAddressesCaptured);
        };

        Because of = () =>
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, Guid.NewGuid());
        };

        It should_transition_to_x2_case_created_state = () =>
        {
            applicationStateMachine.Machine.IsInState(ApplicationState.X2CaseCreated).ShouldBeTrue();
        };

    }
}

