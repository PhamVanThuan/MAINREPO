using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class RemoveDetailFromApplicationAfterNTUFinalisedCommandHandler : IHandlesDomainServiceCommand<RemoveDetailFromApplicationAfterNTUFinalisedCommand>
    {
        private IApplicationRepository applicationRepository;
        private IAccountRepository accountRepository;

        public RemoveDetailFromApplicationAfterNTUFinalisedCommandHandler(IApplicationRepository applicationRepository, IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, RemoveDetailFromApplicationAfterNTUFinalisedCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            int newLegalAgreementDetailTypeKey = accountRepository.GetDetailTypeKeyByDescription(SAHL.Common.Constants.DetailTypes.NewLegalAgreementSigned);
            List<int> detailTypes = new List<int>();
            detailTypes.Add(newLegalAgreementDetailTypeKey);
            applicationRepository.RemoveDetailFromApplication(application, detailTypes);
        }
    }
}