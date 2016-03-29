using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.States
{
    public class when_in_completed_state : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine;
        private static ApplicationState state;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            state = ApplicationState.Completed;
            machine = An<SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>>(state);

            permittedTriggers = new List<ApplicationStateTransitionTrigger> {
                    ApplicationStateTransitionTrigger.CompletionConfirmed
            };

            applicationStateMachine = new ApplicationStateMachine();
        };

        private Because of = () =>
        {
            machine = applicationStateMachine.InitializeMachine(state);
        };

        private It should_be_in_completed_state = () =>
        {
            machine.IsInState(ApplicationState.Completed).ShouldBeTrue();
        };

        private It should_not_permit_any_triggers_to_fire = () =>
        {
            machine.PermittedTriggers.Count().ShouldEqual(0);
        };
    }
}
