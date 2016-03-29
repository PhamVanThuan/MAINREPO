using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.HasApplicationRoleCommandHandlerSpecs
{
    [Subject(typeof(HasApplicationRoleCommandHandler))]
    public class When_application_has_no_applicationrole_for_applicationroletype_return_false : WithFakes
    {
        protected static HasApplicationRoleCommand command;
        protected static HasApplicationRoleCommandHandler handler;
        protected static IDomainMessageCollection messages;

        // Arrange
        Establish context = () =>
            {
                IApplicationRole applicationRole = null;
                var applicationRepository = An<IApplicationRepository>();
                messages = An<IDomainMessageCollection>();

                handler = new HasApplicationRoleCommandHandler(applicationRepository);
                command = new HasApplicationRoleCommand(Param<int>.IsAnything, Param<int>.IsAnything);

                applicationRepository.WhenToldTo(x => x.GetActiveApplicationRoleForTypeAndKey(Param<int>.IsAnything, Param<int>.IsAnything)).Return(applicationRole);
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