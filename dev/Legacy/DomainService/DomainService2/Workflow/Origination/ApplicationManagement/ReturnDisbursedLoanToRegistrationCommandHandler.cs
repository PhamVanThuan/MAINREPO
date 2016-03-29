using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class ReturnDisbursedLoanToRegistrationCommandHandler : IHandlesDomainServiceCommand<ReturnDisbursedLoanToRegistrationCommand>
    {
        IApplicationRepository applicationRepository;
        IDisbursementRepository disbursementRepository;
        ICommonRepository commonRepository;

        public ReturnDisbursedLoanToRegistrationCommandHandler(IApplicationRepository applicationRepository, IDisbursementRepository disbursementRepository, ICommonRepository commonRepository)
        {
            this.applicationRepository = applicationRepository;
            this.disbursementRepository = disbursementRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReturnDisbursedLoanToRegistrationCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            disbursementRepository.ReturnDisbursedLoanToRegistration(application.ReservedAccount.Key);
            commonRepository.RefreshDAOObject<IApplication>(command.ApplicationKey);
        }
    }
}