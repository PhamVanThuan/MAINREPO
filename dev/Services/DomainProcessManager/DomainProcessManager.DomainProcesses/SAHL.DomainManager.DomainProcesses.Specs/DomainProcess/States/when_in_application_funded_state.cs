using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;

using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.States
{
    public class when_in_application_funded_state : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine;
        private static ApplicationState state;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            state = ApplicationState.ApplicationFunded;
            machine = An<SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>>(state);

            permittedTriggers = new List<ApplicationStateTransitionTrigger>
            {
                ApplicationStateTransitionTrigger.ValidApplicationCreationConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported
            };

            applicationStateMachine = new ApplicationStateMachine();
        };

        private Because of = () =>
        {
            machine = applicationStateMachine.InitializeMachine(state);
        };

        private It should_be_in_application_funded_state = () =>
        {
            machine.IsInState(ApplicationState.ApplicationFunded).ShouldBeTrue();
        };

        private It should_only_permit_the_permitted_triggers = () =>
        {
            machine.PermittedTriggers.ShouldContainOnly(permittedTriggers);
        };
    }
}