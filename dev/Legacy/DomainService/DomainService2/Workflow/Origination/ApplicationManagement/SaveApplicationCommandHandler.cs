using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SaveApplicationCommandHandler : IHandlesDomainServiceCommand<SaveApplicationCommand>
    {
        IApplicationRepository applicationRepository;

        public SaveApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SaveApplicationCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            applicationRepository.SaveApplication(application);
            command.Result = true;
        }
    }
}