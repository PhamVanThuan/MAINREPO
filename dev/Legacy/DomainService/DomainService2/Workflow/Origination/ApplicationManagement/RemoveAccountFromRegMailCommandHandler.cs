using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class RemoveAccountFromRegMailCommandHandler : IHandlesDomainServiceCommand<RemoveAccountFromRegMailCommand>
    {
        IRegistrationRepository registrationRepository;
        IApplicationRepository applicationRepository;

        public RemoveAccountFromRegMailCommandHandler(IRegistrationRepository registrationRepository, IApplicationRepository applicationRepository)
        {
            this.registrationRepository = registrationRepository;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, RemoveAccountFromRegMailCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            registrationRepository.DeleteRegmailByAccountKey(application.ReservedAccount.Key);
        }
    }
}