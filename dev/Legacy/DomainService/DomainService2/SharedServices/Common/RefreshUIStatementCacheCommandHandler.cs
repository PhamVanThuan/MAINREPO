using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class RefreshUIStatementCacheCommandHandler : IHandlesDomainServiceCommand<RefreshUIStatementCacheCommand>
    {
        IUIStatementService uiStatementService;

        public RefreshUIStatementCacheCommandHandler(IUIStatementService uiStatementService)
        {
            this.uiStatementService = uiStatementService;
        }

        //this needs to be figured out later
        //needs to flush the UI Statement Cache, but the repo for this uses static methods
        //so need to think about this one for a while.
        public void Handle(IDomainMessageCollection messages, RefreshUIStatementCacheCommand command)
        {
            uiStatementService.ClearCache();
        }
    }
}