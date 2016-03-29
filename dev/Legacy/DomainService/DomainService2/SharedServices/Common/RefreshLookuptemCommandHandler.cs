using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class RefreshLookuptemCommandHandler : IHandlesDomainServiceCommand<RefreshLookupItemCommand>
    {
        private ILookupRepository lookupRepository;

        public RefreshLookuptemCommandHandler(ILookupRepository lookupRepository)
        {
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, RefreshLookupItemCommand command)
        {
            LookupKeys lookup = (LookupKeys)Enum.Parse(typeof(LookupKeys), command.Data.ToString());
            lookupRepository.ResetLookup(lookup);
        }
    }
}