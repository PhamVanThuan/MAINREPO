using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class SuretySignedConfirmedCommandHandler : IHandlesDomainServiceCommand<SuretySignedConfirmedCommand>
    {
        private IApplicationRepository applicationRepository;
        private IAccountRepository accountRepository;

        public SuretySignedConfirmedCommandHandler(IApplicationRepository applicationRepository, IAccountRepository accountRepository)
        {
            this.applicationRepository = applicationRepository;
            this.accountRepository = accountRepository;
        }

        public void Handle(IDomainMessageCollection messages, SuretySignedConfirmedCommand command)
        {
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            accountRepository.CreateOfferRolesNotInAccount(application);
        }
    }
}