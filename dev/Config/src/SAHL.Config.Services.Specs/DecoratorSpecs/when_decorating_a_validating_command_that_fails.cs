using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Core.Specs;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Validation;
using SAHL.Config.Services.Core.Decorators;

namespace SAHL.Config.Services.Specs.DecoratorSpecs
{
    public class when_decorating_a_validating_command_that_fails : WithFakes
    {
        private static ValidateCommandHandlerDecorator<IServiceCommand> _decorator;
        private static IServiceCommandHandler<IServiceCommand> _innerHandler;
        private static ISystemMessageCollection _result;
        private static FakeCommandWithAttributes _command;
        private static IValidateCommand commandValidator;
        private static ITypeMetaDataLookup lookup;
        private static IValidateStrategy strategy;
        private Establish context = () =>
        {
            _innerHandler = An<IServiceCommandHandler<IServiceCommand>>();
            lookup = new TypeMetaDataLookup();
            strategy = new ValidateStrategy(lookup);
            commandValidator = new ValidateCommand(lookup, strategy);
            _command = new FakeCommandWithAttributes();

            _decorator = new ValidateCommandHandlerDecorator<IServiceCommand>(_innerHandler, commandValidator);
        };

        private Because of = () =>
        {
            _result = _decorator.HandleCommand(_command, null);
        };

        private It should_return_error_messages_from_command_handler = () =>
        {
            _result.HasErrors.ShouldEqual(true);
        };

        private It should_not_pass_the_command_to_the_inner_command_handler = () =>
        {
            _decorator.InnerCommandHandler.WasNotToldTo(x => x.HandleCommand(_command, null));
        };

    }
}