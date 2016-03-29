using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ReturnNonDisbursedLoanToProspectCommandHandler : IHandlesDomainServiceCommand<ReturnNonDisbursedLoanToProspectCommand>
    {
        IApplicationRepository applicationRepository;
        ICommonRepository commonRepository;

        public ReturnNonDisbursedLoanToProspectCommandHandler(IApplicationRepository applicationRepository, ICommonRepository commonRepository)
        {
            this.applicationRepository = applicationRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReturnNonDisbursedLoanToProspectCommand command)
        {
            applicationRepository.ReturnNonDisbursedLoanToProspect(command.ApplicationKey);
            commonRepository.RefreshDAOObject<IApplication>(command.ApplicationKey);
        }
    }
}