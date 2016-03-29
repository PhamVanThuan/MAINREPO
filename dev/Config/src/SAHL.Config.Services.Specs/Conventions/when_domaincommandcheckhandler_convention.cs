using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Ioc;

namespace SAHL.Config.Core.Specs.Conventions
{
    public class when_domaincommandcheckhandler_convention : WithFakes
    {
        private static IIocContainer _iocContainer;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {

            var container = TestingIoc.Initialise();
            _iocContainer = container.GetInstance<IIocContainer>();
        };

        private It should_register_fakehandler1_commandcheckhandlers = () =>
        {
            var domainCommandCheckHandlers = _iocContainer.GetInstance<IDomainCommandCheckHandler<IRequiresFake1>>(typeof(IRequiresFake1).Name);
            domainCommandCheckHandlers.ShouldNotBeNull();
        };

        private It should_register_fakehandler2_commandcheckhandlers = () =>
        {
            var domainCommandCheckHandlers = _iocContainer.GetInstance<IDomainCommandCheckHandler<IRequiresFake2>>(typeof(IRequiresFake2).Name);
            domainCommandCheckHandlers.ShouldNotBeNull();
        };
    }
}
