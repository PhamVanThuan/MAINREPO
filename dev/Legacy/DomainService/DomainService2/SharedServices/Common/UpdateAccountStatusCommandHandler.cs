using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class UpdateAccountStatusCommandHandler : IHandlesDomainServiceCommand<UpdateAccountStatusCommand>
    {
        private IAccountRepository accountRepository;

        public UpdateAccountStatusCommandHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateAccountStatusCommand command)
        {
            accountRepository.UpdateAccountStatusWithNoValidation(command.ApplicationKey, command.AccountStatusKey);
        }
    }
}