using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.SharedServices.Common
{
    public class IsRCSAccountCommandHandler : IHandlesDomainServiceCommand<IsRCSAccountCommand>
    {
        protected IAccountRepository accountRepository = null;

        public IsRCSAccountCommandHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsRCSAccountCommand command)
        {
            IAccount account = accountRepository.GetAccountByKey(command.AccountKey);

            command.Result = account.OriginationSource.Key == ((int)SAHL.Common.Globals.OriginationSources.RCS);
        }
    }
}
