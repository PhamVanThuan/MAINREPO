using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.RefreshCacheItemCommandHandlerSpecs
{
    [Subject(typeof(RefreshLookuptemCommandHandler))]
    internal class when_refresh_lookup_item_command_handled : DomainServiceSpec<RefreshLookupItemCommand, RefreshLookuptemCommandHandler>
    {
        protected static ILookupRepository lookupRepository;

        Establish context = () =>
        {
            lookupRepository = An<ILookupRepository>();

            command = new RefreshLookupItemCommand(Param.IsAny<LookupKeys>());
            handler = new RefreshLookuptemCommandHandler(lookupRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_work = () =>
        {
            lookupRepository.WhenToldTo(x => x.ResetLookup(Param.IsAny<LookupKeys>()));
        };
    }
}