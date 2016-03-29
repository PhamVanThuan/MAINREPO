using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler : IHandlesDomainServiceCommand<SetAccountStatusToApplicationPriorToInstructAttorneyCommand>
    {
        IApplicationRepository applicationRepository;
        IAccountRepository accountRepository;
        ICommonRepository commonRepository;

        public SetAccountStatusToApplicationPriorToInstructAttorneyCommandHandler(IApplicationRepository applicationRepository, IAccountRepository accountRepository,ICommonRepository commonRepository)
        {
            this.applicationRepository = applicationRepository;
            this.accountRepository = accountRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SetAccountStatusToApplicationPriorToInstructAttorneyCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application.Account != null)
            {
                // Set HOC account status
                foreach (IAccount acc in application.Account.RelatedChildAccounts)
                {
                    if (acc.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        accountRepository.UpdateAccount(acc.Key, (int)AccountStatuses.ApplicationpriortoInstructAttorney, null, command.ADUserName);
                        commonRepository.RefreshDAOObject<IAccount>(acc.Key);
                    }
                }

                // Update the Main Account
                accountRepository.UpdateAccount(application.Account.Key, (int)AccountStatuses.ApplicationpriortoInstructAttorney, null, command.ADUserName);
                commonRepository.RefreshDAOObject<IAccount>(application.Account.Key);
                commonRepository.RefreshDAOObject<IApplication>(application.Key);
                applicationRepository.SaveApplication(application);
            }
        }
    }
}