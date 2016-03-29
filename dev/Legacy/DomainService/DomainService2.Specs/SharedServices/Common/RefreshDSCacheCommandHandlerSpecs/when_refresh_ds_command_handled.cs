using DomainService2.SharedServices.Common;
using Machine.Specifications;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.RefreshDSCacheCommandHandlerSpecs
{
    [Subject(typeof(RefreshDSCacheCommandHandler))]
    internal class when_refresh_ds_command_handled : DomainServiceSpec<RefreshDSCacheCommand, RefreshDSCacheCommandHandler>
    {
        protected static IX2Service X2Service;
        Establish context = () =>
        {
            X2Service = An<IX2Service>();
            command = new RefreshDSCacheCommand();
            handler = new RefreshDSCacheCommandHandler(X2Service);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_work = () =>
        {
        };
    }
}