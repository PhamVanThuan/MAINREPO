using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class CheckApplicationHasAttributeCommandHandler : IHandlesDomainServiceCommand<CheckApplicationHasAttributeCommand>
    {
        private IApplicationRepository applicationRepository;

        public CheckApplicationHasAttributeCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, CheckApplicationHasAttributeCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application != null)
            {
                command.Result = application.HasAttribute((OfferAttributeTypes)command.ApplicationAttributeTypeKey);
            }
            else
                command.Result = false;
        }
    }
}