using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.States
{
    public class when_in_declarations_captured_state : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine;
        private static ApplicationState state;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            state = ApplicationState.DeclarationsCaptured;

            permittedTriggers = new List<ApplicationStateTransitionTrigger>();
            permittedTriggers.AddRange(Common.NonCriticalPathTriggers);
            permittedTriggers.AddRange(Common.AddressPermittedTriggers);
            permittedTriggers.AddRange(Common.BankAccountPermittedTriggers);

            applicationStateMachine = new ApplicationStateMachine();
        };

        private Because of = () =>
        {
            machine = applicationStateMachine.InitializeMachine(state);
        };

        private It should_be_in_declarations_captured_state = () =>
        {
            machine.IsInState(ApplicationState.DeclarationsCaptured).ShouldBeTrue();
        };

        private It should_only_permit_the_permitted_triggers = () =>
        {
            machine.PermittedTriggers.ShouldContainOnly(permittedTriggers);
        };
    }
}
