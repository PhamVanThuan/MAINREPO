using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CheckUserInMandateCommandHandlerSpecs
{
    [Subject(typeof(CheckUserInMandateCommandHandler))]
    internal class When_CheckUserInMandateCommandHandled_Returns_False : DomainServiceSpec<CheckUserInMandateCommand, CheckUserInMandateCommandHandler>
    {
        protected static IWorkflowAssignmentRepository workflowAssignmentRepository;
        Establish context = () =>
        {
            workflowAssignmentRepository = An<IWorkflowAssignmentRepository>();
            command = new CheckUserInMandateCommand(Param<int>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything);
            handler = new CheckUserInMandateCommandHandler(workflowAssignmentRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}