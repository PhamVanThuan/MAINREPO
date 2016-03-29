using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class RemoveDetailFromAccountCommandHandler : IHandlesDomainServiceCommand<RemoveDetailFromAccountCommand>
    {
        IAccountRepository accountRepository;

        public RemoveDetailFromAccountCommandHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, RemoveDetailFromAccountCommand command)
        {
            IAccount account = accountRepository.GetAccountByApplicationKey(command.ApplicationKey);
            int detailTypeKey = accountRepository.GetDetailTypeKeyByDescription(command.DetailTypeDescription);
            IReadOnlyEventList<IDetail> details = accountRepository.GetDetailByAccountKeyAndDetailType(account.Key, detailTypeKey);

            foreach (IDetail detail in details)
            {
                accountRepository.DeleteDetail(detail);
            }
        }
    }
}