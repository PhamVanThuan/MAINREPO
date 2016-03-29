using System;

using NSubstitute;

using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Config.Core.Specs;
using SAHL.Core.SystemMessages;
using SAHL.Config.Services.Core.Decorators;

namespace SAHL.Config.Services.Specs.DecoratorSpecs
{
    public class when_decorating_a_domain_command_that_must_be_checked_given_no_messages : WithFakes
    {
        private static IIocContainer _iocContainer;
        private static DomainCommandCheckDecorator<IServiceCommand> _decorator;
        private static IServiceCommandHandler<IServiceCommand> _innerHandler;
        private static ISystemMessageCollection _result;
        private static IServiceCommand _command;

        private static RequiresFakeCheckHandler _fakeCheckHandler;

        private Establish context = () =>
        {
            SetupIocContainer();

            _innerHandler = An<IServiceCommandHandler<IServiceCommand>>();
            _decorator = new DomainCommandCheckDecorator<IServiceCommand>(_innerHandler, _iocContainer);
        };

        private Because of = () =>
        {
            _command = new FakeCommand();
            _result = _decorator.HandleCommand(_command, null);
        };

        private It should_handle_requiresfake1 = () =>
        {
            _fakeCheckHandler.IsRequiresFake1Handled.ShouldBeTrue();
        };

        private It should_not_handle_requiresfake2 = () =>
        {
            _fakeCheckHandler.IsRequiresFake2Handled.ShouldBeFalse();
        };

        private It should_return_no_errors = () =>
        {
            _result.AllMessages.ShouldBeEmpty();
        };

        private It should_pass_the_command_to_the_inner_command_handler = () =>
        {
            _innerHandler.WasToldTo(x => x.HandleCommand(_command, null));
        };

        private static void SetupIocContainer()
        {
            _fakeCheckHandler = new RequiresFakeCheckHandler();

            _iocContainer = An<IIocContainer>();
            _iocContainer.GetInstance(typeof(IDomainCommandCheckHandler<IRequiresFake1>), typeof(IRequiresFake1).Name).Returns(_fakeCheckHandler);
            _iocContainer.GetInstance(typeof(IDomainCommandCheckHandler<IRequiresFake2>), typeof(IRequiresFake2).Name).Returns(_fakeCheckHandler);
        }
    }
}
