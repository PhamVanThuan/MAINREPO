using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class Revert30YearTermApplicationToPreviousTermCommandHandler : IHandlesDomainServiceCommand<Revert30YearTermApplicationToPreviousTermCommand>
    {
        private IApplicationRepository applicationRepository;

        public Revert30YearTermApplicationToPreviousTermCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, Revert30YearTermApplicationToPreviousTermCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application != null)
                applicationRepository.RevertToPreviousTermAsRevisedApplication(application);
        }
    }
}