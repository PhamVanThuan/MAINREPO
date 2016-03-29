using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
    public class GetApplicationTypeCommandHandler : IHandlesDomainServiceCommand<GetApplicationTypeCommand>
    {
        private IApplicationRepository applicationRepository;

        public GetApplicationTypeCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetApplicationTypeCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application != null)
            {
                command.ApplicationTypeKeyResult = application.ApplicationType.Key;
            }
            else
            {
                command.ApplicationTypeKeyResult = 0;
            }
        }
    }
}