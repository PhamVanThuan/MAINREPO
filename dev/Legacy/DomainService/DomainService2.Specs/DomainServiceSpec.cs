using Machine.Fakes;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs
{
    public class DomainServiceSpec<T, V> : WithFakes
        where T : IDomainServiceCommand
        where V : IHandlesDomainServiceCommand<T>
    {
        protected static T command;
        protected static V handler;
        protected static IDomainMessageCollection messages;
        protected static ICommandHandler commandHandler;

        public DomainServiceSpec()
        {
            messages = new DomainMessageCollection();
            commandHandler = An<ICommandHandler>();
        }
    }
}