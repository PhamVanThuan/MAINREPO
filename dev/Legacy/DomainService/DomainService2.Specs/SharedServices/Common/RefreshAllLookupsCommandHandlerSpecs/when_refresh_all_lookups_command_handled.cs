using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.X2.Framework;

namespace DomainService2.Specs.SharedServices.Common.RefreshAllLookupsCommandHandlerSpecs
{
    [Subject(typeof(RefreshAllLookupsCommandHandler))]
    internal class when_refresh_all_lookups_command_handled : DomainServiceSpec<RefreshAllLookupsCommand, RefreshAllLookupsCommandHandler>
    {
        protected static ILookupRepository lookupRepository;
        protected static IX2Service x2service;

        Establish context = () =>
        {
            lookupRepository = An<ILookupRepository>();
            x2service = An<IX2Service>();

            command = new RefreshAllLookupsCommand();
            handler = new RefreshAllLookupsCommandHandler(lookupRepository, x2service);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_reset_the_lookups = () =>
        {
            lookupRepository.WhenToldTo(x => x.ResetLookup(Param.IsAny<LookupKeys>()));
        };

        It should_clear_the_domainservice_cache = () =>
        {
            //WorkflowSecurityRepositoryCacheHelper.Instance.WasToldTo(x=>x.ClearCache());
        };
    }
}