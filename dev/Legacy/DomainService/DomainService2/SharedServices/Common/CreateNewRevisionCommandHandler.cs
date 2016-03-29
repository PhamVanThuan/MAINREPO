using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class CreateNewRevisionCommandHandler : IHandlesDomainServiceCommand<CreateNewRevisionCommand>
    {
        private IApplicationRepository applicationRepository;

        public CreateNewRevisionCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, CreateNewRevisionCommand command)
        {
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            application.CreateRevision();
            this.applicationRepository.SaveApplication(application);
        }
    }
}