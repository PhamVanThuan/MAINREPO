using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_DeleteUnconfirmedAffordabilityAssessmentCommand
    {
        private static DeleteUnconfirmedAffordabilityAssessmentCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new DeleteUnconfirmedAffordabilityAssessmentCommand(1);
        };

        private It should_implement_the_unconfirmed_affordability_assessment_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresUnconfirmedAffordabilityAssessment));
        };
    }
}