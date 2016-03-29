using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.Credit.UpdateConditionsCommandHandlerSpecs
{
    [Subject(typeof(UpdateConditionsCommandHandler))]
    public class When_updating_conditions : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static UpdateConditionsCommand command;
        protected static UpdateConditionsCommandHandler handler;
        protected static IConditionsRepository conditionsRepository;
        protected static int applicationKey = 1;

        // Arrange
        Establish context = () =>
        {
            conditionsRepository = An<IConditionsRepository>();
            command = new UpdateConditionsCommand(applicationKey);
            handler = new UpdateConditionsCommandHandler(conditionsRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_execute_update_conditions = () =>
        {
            conditionsRepository.WasToldTo(x => x.UpdateLoanConditions(applicationKey, (int)GenericKeyTypes.Offer));
        };

        // Assert
        It should_set_command_application_key = () =>
        {
            command.ApplicationKey.Equals(applicationKey);
        };
    }
}