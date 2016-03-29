using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Config.Services.Core.Decorators;

namespace SAHL.Config.Core.Specs.DecoratorSpecs
{
    public class when_decorating_a_domain_command_that_must_be_checked : WithFakes
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
            _decorator    = new DomainCommandCheckDecorator<IServiceCommand>(_innerHandler, _iocContainer);
        };

        private Because of = () =>
        {
            _command = new FakeCommand();
            _result = _decorator.HandleCommand(_command, null);
        };

        private It should_return_no_errors = () =>
        {
            _result.AllMessages.ShouldBeEmpty();
        };

        private It should_make_a_call_to_ioc_container_with_correct_type = () =>
        {
            var checkHandlerType        = typeof(IDomainCommandCheckHandler<>);
            var genericCheckHandlerType = checkHandlerType.MakeGenericType(typeof(IRequiresFake1));

            _iocContainer.WasToldTo(container => container.GetInstance(genericCheckHandlerType, typeof (IRequiresFake1).Name));
        };

        private static void SetupIocContainer()
        {
            _fakeCheckHandler = new RequiresFakeCheckHandler();

            _iocContainer = An<IIocContainer>();
            _iocContainer.WhenToldTo(container => container.GetInstance(typeof (IDomainCommandCheckHandler<IRequiresFake1>), typeof (IRequiresFake1).Name)).Return(_fakeCheckHandler);
            _iocContainer.WhenToldTo(container => container.GetInstance(typeof (IDomainCommandCheckHandler<IRequiresFake2>), typeof (IRequiresFake2).Name)).Return(_fakeCheckHandler);
        }
    }
}
