using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.Common
{
    public class ClearRuleCacheCommandHandler : IHandlesDomainServiceCommand<ClearRuleCacheCommand>
    {
        ILookupRepository lookupRepo;
        public ClearRuleCacheCommandHandler(ILookupRepository lookupRepo)
        {
            this.lookupRepo = lookupRepo;
        }
        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ClearRuleCacheCommand command)
        {
            lookupRepo.ClearRuleCache();
        }
    }
}
