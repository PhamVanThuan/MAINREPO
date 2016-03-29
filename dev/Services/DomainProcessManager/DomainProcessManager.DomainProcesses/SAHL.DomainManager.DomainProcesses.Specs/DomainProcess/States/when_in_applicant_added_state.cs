using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.States
{
    public class when_in_applicant_added_state : WithFakes
    {
        private static IApplicationStateMachine applicationStateMachine;
        private static SerialisationFriendlyStateMachine<ApplicationState,ApplicationStateTransitionTrigger> machine;
        private static ApplicationState state;
        private static List<ApplicationStateTransitionTrigger> permittedTriggers;

        private Establish context = () =>
        {
            state = ApplicationState.ApplicantAdded;

            permittedTriggers = new List<ApplicationStateTransitionTrigger> {
                ApplicationStateTransitionTrigger.EmploymentAdditionConfirmed,
                ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed,
                ApplicationStateTransitionTrigger.CriticalErrorReported
            };

            applicationStateMachine = new ApplicationStateMachine();
        };

        private Because of = () =>
        {
            machine = applicationStateMachine.InitializeMachine(state);
        };

        private It should_be_in_applicant_added_state = () =>
        {
            machine.IsInState(ApplicationState.ApplicantAdded).ShouldBeTrue();
        };

        private It should_only_permit_the_permitted_triggers = () =>
        {
            machine.PermittedTriggers.ShouldContainOnly(permittedTriggers);
        };
    }
}
