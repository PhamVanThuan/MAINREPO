using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
    public class GetApplicationStatusCommandHandler : IHandlesDomainServiceCommand<GetApplicationStatusCommand>
    {
        private IApplicationRepository applicationRepository;

        public GetApplicationStatusCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetApplicationStatusCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application != null)
            {
                command.ApplicationStatusKeyResult = application.ApplicationStatus.Key;
            }
            else
            {
                command.ApplicationStatusKeyResult = 0;
            }
        }
    }
}