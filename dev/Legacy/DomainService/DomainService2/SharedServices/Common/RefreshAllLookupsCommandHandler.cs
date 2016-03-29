using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class RefreshAllLookupsCommandHandler : IHandlesDomainServiceCommand<RefreshAllLookupsCommand>
    {
        private ILookupRepository lookupRepository;
        private IX2Service x2Service;

        public RefreshAllLookupsCommandHandler(ILookupRepository lookupRepository, IX2Service x2Service)
        {
            this.lookupRepository = lookupRepository;
            this.x2Service = x2Service;
        }

        public void Handle(IDomainMessageCollection messages, RefreshAllLookupsCommand command)
        {
            lookupRepository.ResetAll();

            WorkflowSecurityRepositoryCacheHelper.Instance.ClearCache();
            //x2Service.ClearDSCache();
        }
    }
}