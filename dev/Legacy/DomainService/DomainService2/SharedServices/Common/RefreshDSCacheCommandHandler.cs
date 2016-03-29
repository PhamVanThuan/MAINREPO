using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class RefreshDSCacheCommandHandler : IHandlesDomainServiceCommand<RefreshDSCacheCommand>
    {
        private IX2Service x2Service;

        public RefreshDSCacheCommandHandler(IX2Service x2Service)
        {
            this.x2Service = x2Service;
        }

        public void Handle(IDomainMessageCollection messages, RefreshDSCacheCommand command)
        {
            //x2Service.ClearDSCache();
            WorkflowSecurityRepositoryCacheHelper.Instance.ClearCache();
        }
    }
}